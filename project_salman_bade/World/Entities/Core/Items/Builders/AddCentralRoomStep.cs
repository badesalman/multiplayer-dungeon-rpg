using System;

namespace ConOOD_RpgGame.World.Entities.Core.Items.Builders
{
    public class AddCentralRoomStep : IBuildStep
    {
        private readonly int width;
        private readonly int height;

        public AddCentralRoomStep(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public void Apply(World world, Random random)
        {
            int startRow = (World.Rows - height) / 2;
            int startCol = (World.Cols - width) / 2;

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