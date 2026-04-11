using ConOOD_RpgGame.World.Entities.Core.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConOOD_RpgGame.World.Entities.Core.Rendering
{
    public class Renderer
    {
        public void Render(World world, Player player, string instructions, string message)
        {
            StringBuilder sb = new StringBuilder();
            int renderWidth = 110;

            List<string> standingLines = WrapText(GetStandingItemText(world, player), 45, 2);

            sb.AppendLine(PadLine("=== STAGE 3 RPG GAME ===", renderWidth));
            sb.AppendLine(PadLine($"Controls: {instructions}", renderWidth));
            sb.AppendLine(PadLine(string.Empty, renderWidth));

            for (int row = 0; row < World.Rows; row++)
            {
                StringBuilder line = new StringBuilder();

                for (int col = 0; col < World.Cols; col++)
                {
                    Position currentPosition = new Position(row, col);
                    Cell cell = world.GetCell(currentPosition);

                    char symbolToDraw;

                    if (player.Position.Row == row && player.Position.Col == col)
                    {
                        symbolToDraw = player.Symbol;
                    }
                    else if (cell.HasEnemy())
                    {
                        symbolToDraw = cell.Enemy.Symbol;
                    }
                    else if (cell.HasItems())
                    {
                        symbolToDraw = cell.PeekTopItem()!.Symbol;
                    }
                    else
                    {
                        symbolToDraw = cell.Ground.Symbol;
                    }

                    line.Append(symbolToDraw);
                }

                if (row == 0)
                    line.Append("    INVENTORY:");
                else if (row >= 1 && row <= 8)
                    line.Append(GetInventoryLine(player, row - 1));
                else if (row == 10)
                    line.Append($"    LEFT HAND : {GetHandText(player.LeftHandItem)}");
                else if (row == 11)
                    line.Append($"    RIGHT HAND: {GetHandText(player.RightHandItem)}");
                else if (row == 13)
                    line.Append("    STANDING ON:");
                else if (row == 14)
                    line.Append($"    {standingLines[0]}");
                else if (row == 15)
                    line.Append($"    {standingLines[1]}");
                else if (row == 16)
                    line.Append($"    STR:{player.GetStrength()} DEX:{player.GetDexterity()} HP:{player.GetCurrentHealth()}");
                else if (row == 17)
                    line.Append($"    LUCK:{player.GetLuck()} AGR:{player.GetAggression()} WIS:{player.GetWisdom()}");
                else if (row == 18)
                    line.Append($"    COINS:{player.Coins}  GOLD:{player.Gold}");
                else if (row == 19)
                    line.Append($"    MSG: {message}");

                sb.AppendLine(PadLine(line.ToString(), renderWidth));
            }

            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.Write(sb.ToString());
        }

        private string PadLine(string text, int width)
        {
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
                string remaining = text.Trim();

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

        private string GetInventoryLine(Player player, int index)
        {
            if (index >= player.Inventory.Count)
            {
                return string.Empty;
            }

            return $"    {index + 1}. {player.Inventory[index].GetDisplayText()}";
        }

        private string GetHandText(Item? item)
        {
            return item == null ? "(empty)" : item.GetDisplayText();
        }

        private string GetStandingItemText(World world, Player player)
        {
            Cell cell = world.GetCell(player.Position);

            if (!cell.HasItems())
            {
                return "Nothing";
            }

            return cell.PeekTopItem()!.GetDetailedInfo();
        }
    }
}