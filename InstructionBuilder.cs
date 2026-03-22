using System.Collections.Generic;

namespace ConOOD_RpgGame
{
    public class InstructionBuilder
    {
        public string Build(World world, Player player)
        {
            List<string> parts = new List<string>();

            parts.Add("W A S D = move");

            Cell currentCell = world.GetCell(player.Position);

            if (currentCell.HasItems())
            {
                parts.Add("E = pick up");
            }

            if (player.Inventory.Count > 0)
            {
                parts.Add("I = inventory menu");
            }

            parts.Add("X = exit");

            return string.Join(" | ", parts);
        }
    }
}