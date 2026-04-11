using System;
namespace ConOOD_RpgGame.World.Entities.Core.Items.Builders
{
    public class AddChambersStep : IBuildStep
    {
        private readonly int chamberCount;
        private readonly int minSize;
        private readonly int maxSize;

        public AddChambersStep(int chamberCount = 4, int minSize = 3, int maxSize = 6)
        {
            this.chamberCount = chamberCount;
            this.minSize = minSize;
            this.maxSize = maxSize;
        }

        public void Apply(World world, Random random)
        {
            for (int i = 0; i < chamberCount; i++)
            {
                int height = random.Next(minSize, maxSize + 1);
                int width = random.Next(minSize, maxSize + 1);

                int startRow = random.Next(0, World.Rows - height + 1);
                int startCol = random.Next(0, World.Cols - width + 1);

                for (int row = startRow; row < startRow + height; row++)
                {
                    for (int col = startCol; col < startCol + width; col++)
                    {
                        world.SetGround(new Position(row, col), new FloorTile());
                    }
                }
            }
        }
    }
}