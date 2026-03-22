using System;

namespace ConOOD_RpgGame
{
    public class EmptyDungeonStep : IBuildStep
    {
        public void Apply(World world, Random random)
        {
            for (int row = 0; row < World.Rows; row++)
            {
                for (int col = 0; col < World.Cols; col++)
                {
                    world.SetGround(new Position(row, col), new FloorTile());
                }
            }
        }
    }
}