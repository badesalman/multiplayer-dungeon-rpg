using System;

namespace ConOOD_RpgGame
{
    public class AddPathsStep : IBuildStep
    {
        private readonly int pathCount;
        private readonly int minLength;
        private readonly int maxLength;

        public AddPathsStep(int pathCount = 4, int minLength = 20, int maxLength = 45)
        {
            this.pathCount = pathCount;
            this.minLength = minLength;
            this.maxLength = maxLength;
        }

        public void Apply(World world, Random random)
        {
            for (int i = 0; i < pathCount; i++)
            {
                int row = random.Next(World.Rows);
                int col = random.Next(World.Cols);
                int length = random.Next(minLength, maxLength + 1);

                for (int step = 0; step < length; step++)
                {
                    world.SetGround(new Position(row, col), new FloorTile());

                    int direction = random.Next(4);

                    switch (direction)
                    {
                        case 0: row--; break;
                        case 1: row++; break;
                        case 2: col--; break;
                        case 3: col++; break;
                    }

                    if (row < 0) row = 0;
                    if (row >= World.Rows) row = World.Rows - 1;
                    if (col < 0) col = 0;
                    if (col >= World.Cols) col = World.Cols - 1;
                }
            }
        }
    }
}