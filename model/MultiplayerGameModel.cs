using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using ConOOD_RpgGame.Network.DTO;
using ConOOD_RpgGame.World.Entities;
using ConOOD_RpgGame.World.Entities.Core;
using ConOOD_RpgGame.World.Entities.Core.combat;
using ConOOD_RpgGame.World.Entities.Core.Items.Builders.Dungeon;
using ConOOD_RpgGame.World.Entities.Core.Items.Themes;
using ConOOD_RpgGame.World.Entities.Core.Logging;
using ConOOD_RpgGame.World.Enums;

namespace ConOOD_RpgGame.Model
{
    public class MultiplayerGameModel
    {
        private readonly object modelLock = new object();
        private readonly ConcurrentQueue<PlayerActionCommand> actionQueue;

        private readonly SoundManager soundManager;
        private readonly CombatSystem combatSystem;

        private string lastMessage;
        private readonly IDungeonTheme theme;
        public ConOOD_RpgGame.World.World World { get; }
        public Dictionary<int, Player> Players { get; }
        private bool gameOver;
        public MultiplayerGameModel()
        {
            soundManager = new SoundManager();
            combatSystem = new CombatSystem();
            actionQueue = new ConcurrentQueue<PlayerActionCommand>();

            Players = new Dictionary<int, Player>();
            World = new ConOOD_RpgGame.World.World();

            DungeonBuilder builder = new DungeonBuilder();
            theme = DungeonThemeFactory.CreateRandomTheme();

            theme.ConfigureDungeon(builder);
            builder.Build(World);
            theme.AddThemeContent(World);

            lastMessage = $"{theme.GetThemeName()} - {theme.GetIntroMessage()}";
            GameLogger.Instance.Log($"Multiplayer theme selected: {theme.GetThemeName()}");
        }

        public int AddPlayer()
        {
            lock (modelLock)
            {
                if (Players.Count >= 9)
                {
                    return -1;
                }

                int id = FindFreePlayerId();
                Position position = FindFreePosition();

                Player player = new Player(position, soundManager);
                Players.Add(id, player);

                lastMessage = $"Player {id} joined.";
                GameLogger.Instance.Log(lastMessage);

                return id;
            }
        }

        public void RemovePlayer(int playerId)
        {
            lock (modelLock)
            {
                if (Players.ContainsKey(playerId))
                {
                    Players.Remove(playerId);
                    lastMessage = $"Player {playerId} left.";
                    GameLogger.Instance.Log(lastMessage);
                }
            }
        }

        public void EnqueueAction(int playerId, string action)
        {
            actionQueue.Enqueue(new PlayerActionCommand(playerId, action));
        }

        public bool ProcessNextAction()
        {
            if (!actionQueue.TryDequeue(out PlayerActionCommand command))
            {
                return false;
            }

            HandleAction(command.PlayerId, command.Action);
            return true;
        }

        public void HandleAction(int playerId, string action)
        {
            lock (modelLock)
            {
                if (gameOver)
                {
                    lastMessage = "Game Over.";
                    return;
                }
                if (!Players.ContainsKey(playerId))
                {
                    lastMessage = "Unknown player.";
                    return;
                }

                Player player = Players[playerId];
                string message;

                if (action == "UP")
                {
                    player.Move(World, -1, 0, out message);
                    MoveEnemies();
                }
                else if (action == "DOWN")
                {
                    player.Move(World, 1, 0, out message);
                    MoveEnemies();
                }
                else if (action == "LEFT")
                {
                    player.Move(World, 0, -1, out message);
                    MoveEnemies();
                }
                else if (action == "RIGHT")
                {
                    player.Move(World, 0, 1, out message);
                    MoveEnemies();
                }
                else if (action == "PICKUP")
                {
                    player.PickUpTopItem(World, out message);
                    MoveEnemies();
                }
                else if (action == "INVENTORY")
                {
                    message = "Inventory opened.";
                }
                else if (action.StartsWith("INV_"))
                {
                    message = HandleInventoryCommand(player, action.Substring(4));
                }
                else if (action == "ATTACK_NORMAL")
                {
                    message = TryAttackEnemy(player, new NormalAttack());
                    MoveEnemies();
                }
                else if (action == "ATTACK_STEALTH")
                {
                    message = TryAttackEnemy(player, new StealthAttack());
                    MoveEnemies();
                }
                else if (action == "ATTACK_MAGICAL")
                {
                    message = TryAttackEnemy(player, new MagicalAttack());
                    MoveEnemies();
                }
                else
                {
                    message = "Unknown action.";
                }

                lastMessage = $"Player {playerId}: {message}";
                GameLogger.Instance.Log(lastMessage);
            }
        }

        public GameStateDto CreateStateDto(int viewerPlayerId)
        {
            lock (modelLock)
            {
                GameStateDto dto = new GameStateDto();

                dto.Rows = ConOOD_RpgGame.World.World.Rows;
                dto.Cols = ConOOD_RpgGame.World.World.Cols;
                dto.Map = new char[dto.Rows][];
                dto.Message = lastMessage;
                dto.ThemeLine = $"{theme.GetThemeName()} - {theme.GetIntroMessage()}";
                dto.Instructions = "W A S D = move | 1 = normal attack | 2 = stealth attack | 3 = magical attack | X = exit";
                dto.RecentLogs = GameLogger.Instance.GetRecentLogs(5);

                for (int row = 0; row < dto.Rows; row++)
                {
                    dto.Map[row] = new char[dto.Cols];

                    for (int col = 0; col < dto.Cols; col++)
                    {
                        Position position = new Position(row, col);
                        Cell cell = World.GetCell(position);

                        if (cell.HasEnemy())
                        {
                            dto.Map[row][col] = cell.Enemy.Symbol;
                        }
                        else if (cell.HasItems())
                        {
                            dto.Map[row][col] = cell.PeekTopItem().Symbol;
                        }
                        else
                        {
                            dto.Map[row][col] = cell.Ground.Symbol;
                        }
                    }
                }

                foreach (var pair in Players)
                {
                    Player player = pair.Value;

                    dto.Players.Add(new PlayerDto
                    {
                        Id = pair.Key,
                        Row = player.Position.Row,
                        Col = player.Position.Col,
                        Health = player.GetCurrentHealth(),
                        Coins = player.Coins,
                        Gold = player.Gold
                    });

                    dto.Map[player.Position.Row][player.Position.Col] =
                        pair.Key.ToString()[0];
                }

                if (Players.ContainsKey(viewerPlayerId))
                {
                    Player viewer = Players[viewerPlayerId];

                    dto.InventoryLines = GetInventoryLines(viewer);
                    dto.InventoryText = BuildInventoryText(viewer);
                    dto.LeftHandText = viewer.LeftHandItem == null ? "(empty)" : viewer.LeftHandItem.GetDisplayText();
                    dto.RightHandText = viewer.RightHandItem == null ? "(empty)" : viewer.RightHandItem.GetDisplayText();
                    dto.StandingOnText = GetStandingItemText(viewer);

                    dto.Strength = viewer.GetStrength();
                    dto.Dexterity = viewer.GetDexterity();
                    dto.Luck = viewer.GetLuck();
                    dto.Aggression = viewer.GetAggression();
                    dto.Wisdom = viewer.GetWisdom();
                    dto.Health = viewer.GetCurrentHealth();
                    dto.Coins = viewer.Coins;
                    dto.Gold = viewer.Gold;
                }

                return dto;
            }
        }

        private string TryAttackEnemy(Player player, AttackStrategy strategy)
        {
            Enemy enemy = FindAdjacentEnemy(player);

            if (enemy == null)
            {
                return "There is no enemy nearby.";
            }

            string result = combatSystem.ExecuteTurn(player, enemy, strategy);

            if (!player.IsAlive)
            {
                Console.Clear();
                Console.WriteLine("GAME OVER");
                Environment.Exit(0);
            }

            if (!enemy.IsAlive)
            {
                World.RemoveEnemy(enemy.Position);
            }

            return result;
        }

        private Enemy FindAdjacentEnemy(Player player)
        {
            List<Position> positions = new List<Position>
    {
        player.Position.Offset(-1, 0),
        player.Position.Offset(1, 0),
        player.Position.Offset(0, -1),
        player.Position.Offset(0, 1)
    };

            foreach (Position position in positions)
            {
                if (!World.IsInside(position))
                {
                    continue;
                }

                Cell cell = World.GetCell(position);

                if (cell.HasEnemy())
                {
                    return cell.Enemy;
                }
            }

            return null;
        }

        private void MoveEnemies()
        {
            List<Enemy> enemiesToMove = new List<Enemy>();

            for (int row = 0; row < ConOOD_RpgGame.World.World.Rows; row++)
            {
                for (int col = 0; col < ConOOD_RpgGame.World.World.Cols; col++)
                {
                    Cell cell = World.GetCell(new Position(row, col));

                    if (cell.HasEnemy())
                    {
                        enemiesToMove.Add(cell.Enemy);
                    }
                }
            }

            foreach (Enemy enemy in enemiesToMove)
            {
                if (enemy.IsAlive)
                {
                    enemy.TakeTurn(Players.Values);

                    foreach (Player player in Players.Values)
                    {
                        if (!player.IsAlive)
                        {
                            gameOver = true;
                            lastMessage = "Game Over.";
                            return;
                        }
                    }
                }
            }
        }

        private string BuildInventoryText(Player player)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("=== INVENTORY MENU ===");
            sb.AppendLine();
            sb.AppendLine("Inventory:");

            if (player.Inventory.Count == 0)
            {
                sb.AppendLine("(empty)");
            }
            else
            {
                for (int i = 0; i < player.Inventory.Count; i++)
                {
                    sb.AppendLine($"{i + 1}. {player.Inventory[i].GetDisplayText()}");
                }
            }

            sb.AppendLine();
            sb.AppendLine($"Left hand : {(player.LeftHandItem == null ? "(empty)" : player.LeftHandItem.GetDisplayText())}");
            sb.AppendLine($"Right hand: {(player.RightHandItem == null ? "(empty)" : player.RightHandItem.GetDisplayText())}");
            sb.AppendLine();
            sb.AppendLine("Commands:");
            sb.AppendLine("L <number>  -> equip item to LEFT hand");
            sb.AppendLine("R <number>  -> equip item to RIGHT hand");
            sb.AppendLine("D <number>  -> drop item");
            sb.AppendLine("UL          -> unequip LEFT hand");
            sb.AppendLine("UR          -> unequip RIGHT hand");
            sb.AppendLine("Q           -> back");

            return sb.ToString();
        }

        private string HandleInventoryCommand(Player player, string input)
        {
            input = input.Trim().ToUpper();

            if (input == "UL")
            {
                player.Unequip(HandSide.Left, out string message);
                return message;
            }

            if (input == "UR")
            {
                player.Unequip(HandSide.Right, out string message);
                return message;
            }

            string[] parts = input.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
            {
                return "Invalid command format.";
            }

            string action = parts[0];

            if (!int.TryParse(parts[1], out int index))
            {
                return "Invalid inventory number.";
            }

            index -= 1;

            if (action == "D")
            {
                player.DropItem(World, index, out string message);
                return message;
            }

            if (action == "L")
            {
                player.EquipItem(index, HandSide.Left, out string message);
                return message;
            }

            if (action == "R")
            {
                player.EquipItem(index, HandSide.Right, out string message);
                return message;
            }

            return "Unknown inventory command.";
        }

        private int FindFreePlayerId()
        {
            for (int id = 1; id <= 9; id++)
            {
                if (!Players.ContainsKey(id))
                {
                    return id;
                }
            }

            return -1;
        }

        private Position FindFreePosition()
        {
            for (int row = 0; row < ConOOD_RpgGame.World.World.Rows; row++)
            {
                for (int col = 0; col < ConOOD_RpgGame.World.World.Cols; col++)
                {
                    Position position = new Position(row, col);

                    if (!World.CanEnter(position))
                    {
                        continue;
                    }

                    bool occupied = false;

                    foreach (Player player in Players.Values)
                    {
                        if (player.Position.Row == row && player.Position.Col == col)
                        {
                            occupied = true;
                        }
                    }

                    if (!occupied)
                    {
                        return position;
                    }
                }
            }

            return new Position(0, 0);
        }

        private class PlayerActionCommand
        {
            public int PlayerId { get; }
            public string Action { get; }

            public PlayerActionCommand(int playerId, string action)
            {
                PlayerId = playerId;
                Action = action;
            }
        }
        private List<string> GetInventoryLines(Player player)
        {
            List<string> lines = new List<string>();

            for (int i = 0; i < player.Inventory.Count && i < 4; i++)
            {
                lines.Add($"{i + 1}. {player.Inventory[i].GetDisplayText()}");
            }

            return lines;
        }

        private string GetStandingItemText(Player player)
        {
            Cell cell = World.GetCell(player.Position);

            if (!cell.HasItems())
            {
                return "Nothing";
            }

            return cell.PeekTopItem().GetDetailedInfo();
        }
    }
}