using System;
using System.Collections.Generic;

namespace ConOOD_RpgGame
{
    public class AddItemsStep : IBuildStep
    {
        private readonly int itemCount;

        public AddItemsStep(int itemCount)
        {
            this.itemCount = itemCount;
        }

        public void Apply(World world, Random random)
        {
            string[] names =
            {
                "Old Boot",
                "Broken Key",
                "Cracked Orb",
                "Dusty Note",
                "Rusty Spoon"
            };

            string[] descriptions =
            {
                "A worn-out boot.",
                "It looks useless now.",
                "A strange but unusable item.",
                "The text is unreadable.",
                "Completely useless."
            };

            List<Position> positions = GetPassablePositions(world);

            for (int i = 0; i < itemCount && positions.Count > 0; i++)
            {
                Position pos = TakeRandomPosition(positions, random);
                int itemIndex = random.Next(names.Length);

                world.AddItem(pos, new JunkItem(names[itemIndex], '?', descriptions[itemIndex]));
            }
        }

        private List<Position> GetPassablePositions(World world)
        {
            List<Position> result = new List<Position>();

            for (int row = 0; row < World.Rows; row++)
            {
                for (int col = 0; col < World.Cols; col++)
                {
                    Position pos = new Position(row, col);

                    if (world.CanEnter(pos))
                    {
                        result.Add(pos);
                    }
                }
            }

            return result;
        }

        private Position TakeRandomPosition(List<Position> positions, Random random)
        {
            int index = random.Next(positions.Count);
            Position pos = positions[index];
            positions.RemoveAt(index);
            return pos;
        }
    }
}
