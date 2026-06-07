using System;
using System.Collections.Generic;
using System.Text;
using ConOOD_RpgGame.Network.DTO;

namespace ConOOD_RpgGame.Views
{
    public class ConsoleGameView
    {
        public void Render(GameStateDto state)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = false;

            StringBuilder sb = new StringBuilder();
            int renderWidth = 120;

            sb.AppendLine(PadLine("=== MULTIPLAYER RPG GAME ===", renderWidth));
            sb.AppendLine(PadLine(state.ThemeLine, renderWidth));
            sb.AppendLine(PadLine(state.Instructions, renderWidth));
            sb.AppendLine(PadLine(string.Empty, renderWidth));

            List<string> standingLines = WrapText(state.StandingOnText, 45, 2);

            for (int row = 0; row < state.Rows; row++)
            {
                StringBuilder line = new StringBuilder();

                for (int col = 0; col < state.Cols; col++)
                {
                    line.Append(state.Map[row][col]);
                }

                if (row == 0)
                    line.Append("    INVENTORY:");
                else if (row >= 1 && row <= 4)
                    line.Append(GetInventoryLine(state, row - 1));
                else if (row == 5)
                    line.Append($"    LEFT HAND : {state.LeftHandText}");
                else if (row == 6)
                    line.Append($"    RIGHT HAND: {state.RightHandText}");
                else if (row == 8)
                    line.Append("    STANDING ON:");
                else if (row == 9)
                    line.Append($"    {standingLines[0]}");
                else if (row == 10)
                    line.Append($"    {standingLines[1]}");
                else if (row == 12)
                    line.Append($"    STR:{state.Strength} DEX:{state.Dexterity} HP:{state.Health}");
                else if (row == 13)
                    line.Append($"    LUCK:{state.Luck} AGR:{state.Aggression} WIS:{state.Wisdom}");
                else if (row == 14)
                    line.Append($"    COINS:{state.Coins}  GOLD:{state.Gold}");
                else if (row == 15)
                    line.Append($"    MSG: {GetMessageLine(state.Message, 0)}");
                else if (row == 16)
                    line.Append($"    {GetMessageLine(state.Message, 1)}");
                else if (row == 17)
                    line.Append("    RECENT LOGS:");
                else if (row >= 18 && row < 18 + state.RecentLogs.Count)
                    line.Append($"    {Clean(state.RecentLogs[row - 18])}");

                sb.AppendLine(PadLine(line.ToString(), renderWidth));
            }

            Console.Write(sb.ToString());
        }

        public void ShowInventoryPrompt()
        {
            Console.Clear();
            Console.CursorVisible = true;

            Console.WriteLine("=== INVENTORY MENU ===");
            Console.WriteLine();
            Console.WriteLine("Commands:");
            Console.WriteLine("L <number>  -> equip item to LEFT hand");
            Console.WriteLine("R <number>  -> equip item to RIGHT hand");
            Console.WriteLine("D <number>  -> drop item");
            Console.WriteLine("UL          -> unequip LEFT hand");
            Console.WriteLine("UR          -> unequip RIGHT hand");
            Console.WriteLine("Q           -> back");
            Console.WriteLine();
            Console.Write("Command: ");
        }

        public void HideCursor()
        {
            Console.CursorVisible = false;
        }

        private string GetInventoryLine(GameStateDto state, int index)
        {
            if (index >= state.InventoryLines.Count)
            {
                return string.Empty;
            }

            return $"    {state.InventoryLines[index]}";
        }

        private string PadLine(string text, int width)
        {
            if (text == null)
            {
                text = "";
            }

            text = Clean(text);

            if (text.Length > width)
            {
                return text.Substring(0, width);
            }

            return text.PadRight(width);
        }

        private List<string> WrapText(string text, int width, int maxLines)
        {
            List<string> lines = new List<string>();

            if (string.IsNullOrWhiteSpace(text))
            {
                lines.Add(string.Empty);
            }
            else
            {
                string remaining = Clean(text).Trim();

                while (remaining.Length > 0 && lines.Count < maxLines)
                {
                    if (remaining.Length <= width)
                    {
                        lines.Add(remaining);
                        remaining = string.Empty;
                        break;
                    }

                    int splitIndex = remaining.LastIndexOf(' ', width);

                    if (splitIndex <= 0)
                    {
                        splitIndex = width;
                    }

                    lines.Add(remaining.Substring(0, splitIndex).TrimEnd());
                    remaining = remaining.Substring(splitIndex).TrimStart();
                }
            }

            while (lines.Count < maxLines)
            {
                lines.Add(string.Empty);
            }

            return lines;
        }

        private string Clean(string text)
        {
            if (text == null)
            {
                return "";
            }

            return text.Replace("\r", " ").Replace("\n", " ");
        }
        private string Shorten(string text, int maxLength)
        {
            if (text == null)
            {
                return "";
            }

            if (text.Length <= maxLength)
            {
                return text;
            }

            return text.Substring(0, maxLength);
        }

        private string GetMessageLine(string message, int lineIndex)
        {
            message = Clean(message);

            int maxLength = 45;
            int start = lineIndex * maxLength;

            if (message.Length <= start)
            {
                return "";
            }

            int length = message.Length - start;

            if (length > maxLength)
            {
                length = maxLength;
            }

            return message.Substring(start, length);
        }
    }
}