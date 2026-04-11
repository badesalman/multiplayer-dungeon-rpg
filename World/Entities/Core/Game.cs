using ConOOD_RpgGame.World.Controllers;
using ConOOD_RpgGame.World.Entities;
using ConOOD_RpgGame.World.Entities.Core.combat;
using ConOOD_RpgGame.World.Entities.Core.Items.Builders;
using ConOOD_RpgGame.World.Entities.Core.Items.Builders.Dungeon;
using ConOOD_RpgGame.World.Entities.Core.Rendering;
using ConOOD_RpgGame.World.Enums;
using System;

namespace ConOOD_RpgGame.World.Entities.Core
{
    public class Game
    {
        private readonly World world;
        private readonly Player player;
        private readonly Renderer renderer;
        private readonly KeyboardController keyboardController;
        private readonly InstructionBuilder instructionBuilder;
        private readonly CombatSystem combatSystem;
        private string lastMessage;

        public Game()
        {
            world = new World();

            DungeonBuilder builder = new DungeonBuilder();
            IDungeonStrategy strategy = new DungeonGroundsStrategy();
            strategy.Configure(builder);
            builder.Build(world);

            Position startPosition = FindFirstPassablePosition(world);
            player = new Player(startPosition);

            renderer = new Renderer();
            keyboardController = new KeyboardController(player, world, OpenInventoryMenu);
            instructionBuilder = new InstructionBuilder();

            combatSystem = new CombatSystem();
            lastMessage = "Welcome to the game.";
        }

        public void Run()
        {
            bool isDead = false;
            bool running = true;

            while (running)
            {

                if (player.GetCurrentHealth() <= 0)
                {
                    Console.Clear();
                    Console.WriteLine("GAME OVER - You died.");
                    Console.ReadKey();
                    isDead = true;
                    break;
                }

                string instructions = instructionBuilder.Build(world, player);
                renderer.Render(world, player, instructions, lastMessage);

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.D1)
                {
                    lastMessage = TryAttackEnemy(new NormalAttack());
                }
                else if (keyInfo.Key == ConsoleKey.D2)
                {
                    lastMessage = TryAttackEnemy(new StealthAttack());
                }
                else if (keyInfo.Key == ConsoleKey.D3)
                {
                    lastMessage = TryAttackEnemy(new MagicalAttack());
                }
                else
                {
                    lastMessage = keyboardController.HandleKey(keyInfo.Key);

                    if (keyboardController.ExitRequested)
                    {
                        running = false;
                    }
                }
            }

            Console.Clear();
            if (!isDead)
            {
                Console.WriteLine("Game closed.");
                Console.ReadKey();
            }
        }

        private Position FindFirstPassablePosition(World world)
        {
            for (int row = 0; row < World.Rows; row++)
            {
                for (int col = 0; col < World.Cols; col++)
                {
                    Position pos = new Position(row, col);

                    if (world.CanEnter(pos))
                    {
                        return pos;
                    }
                }
            }

            return new Position(0, 0);
        }

        private string OpenInventoryMenu()
        {
            while (true)
            {
                Console.Clear();
                PrintInventoryMenu();

                Console.Write("Command: ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    return "Empty command.";
                }

                input = input.Trim().ToUpper();

                if (input == "Q")
                {
                    return "Closed inventory menu.";
                }

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

                string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2)
                {
                    return "Invalid command format.";
                }

                string action = parts[0];
                bool parsed = int.TryParse(parts[1], out int index);

                if (!parsed)
                {
                    return "Invalid inventory number.";
                }

                index -= 1;

                if (action == "D")
                {
                    player.DropItem(world, index, out string message);
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
        }

        private string TryAttackEnemy(AttackStrategy strategy)
        {
            Cell cell = world.GetCell(player.Position);

            if (!cell.HasEnemy())
            {
                return "There is no enemy here.";
            }

            Enemy enemy = cell.Enemy;
            string result = combatSystem.ExecuteTurn(player, enemy, strategy);

            if (!enemy.IsAlive)
            {
                world.RemoveEnemy(player.Position);
            }

            return result;
        }

        private void PrintInventoryMenu()
        {
            Console.WriteLine("=== INVENTORY MENU ===");
            Console.WriteLine();
            Console.WriteLine("Inventory:");

            if (player.Inventory.Count == 0)
            {
                Console.WriteLine("(empty)");
            }
            else
            {
                for (int i = 0; i < player.Inventory.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {player.Inventory[i].GetDisplayText()}");
                }
            }

            Console.WriteLine();
            Console.WriteLine($"Left hand : {(player.LeftHandItem == null ? "(empty)" : player.LeftHandItem.GetDisplayText())}");
            Console.WriteLine($"Right hand: {(player.RightHandItem == null ? "(empty)" : player.RightHandItem.GetDisplayText())}");
            Console.WriteLine();
            Console.WriteLine("Commands:");
            Console.WriteLine("L <number>  -> equip item to LEFT hand");
            Console.WriteLine("R <number>  -> equip item to RIGHT hand");
            Console.WriteLine("D <number>  -> drop item");
            Console.WriteLine("UL          -> unequip LEFT hand");
            Console.WriteLine("UR          -> unequip RIGHT hand");
            Console.WriteLine("Q           -> back");
            Console.WriteLine();
        }
    }
}