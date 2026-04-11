using ConOOD_RpgGame.World.Entities.Core;
using System;
using System.Collections.Generic;

namespace ConOOD_RpgGame.World.Entities.Core.Items.Builders
{
    public class AddEnemiesStep : IBuildStep
    {
        private readonly int enemyCount;

        public AddEnemiesStep(int enemyCount)
        {
            this.enemyCount = enemyCount;
        }

        public void Apply(World world, Random random)
        {
            List<Position> positions = GetPassablePositions(world);

            for (int i = 0; i < enemyCount && positions.Count > 0; i++)
            {
                Position pos = TakeRandomPosition(positions, random);

                if (world.HasEnemyAt(pos))
                {
                    i--;
                    continue;
                }

                Enemy enemy = CreateRandomEnemy(pos, random);
                world.AddEnemy(pos, enemy);
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

        private Enemy CreateRandomEnemy(Position position, Random random)
        {
            int choice = random.Next(3);

            if (choice == 0)
            {
                return new Enemy(position, 30, 4, 2);
            }

            if (choice == 1)
            {
                return new Enemy(position, 40, 5, 3);
            }

            return new Enemy(position, 50, 6, 4);
        }
    }
}