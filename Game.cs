using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConOOD_RpgGame
{
    public class Game
    {
        private readonly World world;
        private readonly Player player;
        private readonly Renderer renderer;
        private string lastMessage;

        public Game()
        {
            world = new World();
            player = new Player(new Position(0, 0));
            renderer = new Renderer();
            lastMessage = "Welcome to the game.";
        }

        public void Run()
        {
            bool running = true;

            while (running)
            {
                renderer.Render(world, player, lastMessage);

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.W:
                        player.Move(world, -1, 0, out lastMessage);
                        break;

                    case ConsoleKey.S:
                        player.Move(world, 1, 0, out lastMessage);
                        break;

                    case ConsoleKey.A:
                        player.Move(world, 0, -1, out lastMessage);
                        break;

                    case ConsoleKey.D:
                        player.Move(world, 0, 1, out lastMessage);
                        break;

                    case ConsoleKey.E:
                        player.PickUpTopItem(world, out lastMessage);
                        break;

                    case ConsoleKey.I:
                        OpenInventoryMenu();
                        break;

                    case ConsoleKey.X:
                        running = false;
                        break;

                    default:
                        lastMessage = "Unknown key.";
                        break;
                }
            }

            Console.Clear();
            Console.WriteLine("Game closed.");
        }

        private void OpenInventoryMenu()
        {
            while (true)
            {
                Console.Clear();
                PrintInventoryMenu();

                Console.Write("Command: ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    lastMessage = "Empty command.";
                    return;
                }

                input = input.Trim().ToUpper();

                if (input == "Q")
                {
                    lastMessage = "Closed inventory menu.";
                    return;
                }

                if (input == "UL")
                {
                    player.Unequip(HandSide.Left, out lastMessage);
                    return;
                }

                if (input == "UR")
                {
                    player.Unequip(HandSide.Right, out lastMessage);
                    return;
                }

                string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2)
                {
                    lastMessage = "Invalid command format.";
                    return;
                }

                string action = parts[0];
                bool parsed = int.TryParse(parts[1], out int index);

                if (!parsed)
                {
                    lastMessage = "Invalid inventory number.";
                    return;
                }

                index -= 1;

                if (action == "D")
                {
                    player.DropItem(world, index, out lastMessage);
                    return;
                }

                if (action == "L")
                {
                    player.EquipItem(index, HandSide.Left, out lastMessage);
                    return;
                }

                if (action == "R")
                {
                    player.EquipItem(index, HandSide.Right, out lastMessage);
                    return;
                }

                lastMessage = "Unknown inventory command.";
                return;
            }
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
