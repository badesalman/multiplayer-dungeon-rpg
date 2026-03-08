using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConOOD_RpgGame
{
    public class Renderer
    {
        public void Render(World world, Player player, string message)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("=== STAGE 1 RPG GAME ===");
            sb.AppendLine("Controls: W A S D = move | E = pick up | I = inventory menu | X = exit");
            sb.AppendLine();

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
                        symbolToDraw = '@';
                    }
                    else if (cell.HasItems())
                    {
                        symbolToDraw = cell.PeekTopItem().Symbol;
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
                    line.Append($"    {GetStandingItemText(world, player)}");
                else if (row == 16)
                    line.Append($"    STR:{player.Strength} DEX:{player.Dexterity} HP:{player.Health}");
                else if (row == 17)
                    line.Append($"    LUCK:{player.Luck} AGR:{player.Aggression} WIS:{player.Wisdom}");
                else if (row == 18)
                    line.Append($"    COINS:{player.Coins}  GOLD:{player.Gold}");
                else if (row == 19)
                    line.Append($"    MSG: {message}");

                sb.AppendLine(line.ToString());
            }

            Console.SetCursorPosition(0, 0);
            Console.Write(sb.ToString());
        }

        private string GetInventoryLine(Player player, int index)
        {
            if (index >= player.Inventory.Count)
            {
                return string.Empty;
            }

            return $"    {index + 1}. {player.Inventory[index].GetDisplayText()}";
        }

        private string GetHandText(Item item)
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

            return cell.PeekTopItem().GetDetailedInfo();
        }
    }
}
