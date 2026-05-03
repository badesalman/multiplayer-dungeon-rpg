using ConOOD_RpgGame.World.Entities.Core.Logging;
using ConOOD_RpgGame.World.Entities;
using System;
using System.Collections.Generic;

namespace ConOOD_RpgGame.World.Controllers
{
    public class KeyboardController
    {
        private readonly Dictionary<ConsoleKey, Func<string>> actions;

        public bool ExitRequested { get; private set; }

        public KeyboardController(Player player, World world, Func<string> inventoryMenuAction)
        {
            actions = new Dictionary<ConsoleKey, Func<string>>();

            actions[ConsoleKey.W] = () =>
            {
                player.Move(world, -1, 0, out string message);
                GameLogger.Instance.Log(message);
                return message;
            };

            actions[ConsoleKey.S] = () =>
            {
                player.Move(world, 1, 0, out string message);
                GameLogger.Instance.Log(message);
                return message;
            };

            actions[ConsoleKey.A] = () =>
            {
                player.Move(world, 0, -1, out string message);
                GameLogger.Instance.Log(message);
                return message;
            };

            actions[ConsoleKey.D] = () =>
            {
                player.Move(world, 0, 1, out string message);
                GameLogger.Instance.Log(message);
                return message;
            };

            actions[ConsoleKey.E] = () =>
            {
                player.PickUpTopItem(world, out string message);
                GameLogger.Instance.Log(message);
                return message;
            };

            actions[ConsoleKey.I] = inventoryMenuAction;

            actions[ConsoleKey.X] = () =>
            {
                ExitRequested = true;
                GameLogger.Instance.Log("Game exited.");
                return "Exiting game.";
            };
        }

        public string HandleKey(ConsoleKey key)
        {
            if (actions.TryGetValue(key, out Func<string> action))
            {
                return action();
            }

            GameLogger.Instance.Log("Unknown key pressed.");
            return "Unknown key.";
        }
    }
}