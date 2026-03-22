using System;
using System.Collections.Generic;

namespace ConOOD_RpgGame
{
    public class AddWeaponsStep : IBuildStep
    {
        private readonly int weaponCount;

        public AddWeaponsStep(int weaponCount)
        {
            this.weaponCount = weaponCount;
        }

        public void Apply(World world, Random random)
        {
            List<Position> positions = GetPassablePositions(world);

            for (int i = 0; i < weaponCount && positions.Count > 0; i++)
            {
                Position pos = TakeRandomPosition(positions, random);
                world.AddItem(pos, CreateRandomWeapon(random));
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

        private Weapon CreateRandomWeapon(Random random)
        {
            int choice = random.Next(3);

            if (choice == 0)
            {
                return new Weapon("Dagger", '/', "A small quick blade.", 4, false);
            }

            if (choice == 1)
            {
                return new Weapon("Sword", '!', "A balanced one-handed sword.", 7, false);
            }

            return new Weapon("Great Axe", '%', "A heavy two-handed axe.", 12, true);
        }
    }
}