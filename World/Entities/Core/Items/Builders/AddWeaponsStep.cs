using ConOOD_RpgGame.World.Entities.Core.Items.Modifiers;
using ConOOD_RpgGame.World.Entities.Core.Items;
using System;
using System.Collections.Generic;

namespace ConOOD_RpgGame.World.Entities.Core.Items.Builders
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

        private Item CreateRandomWeapon(Random random)
        {
            Weapon weapon;
            int choice = random.Next(4);

            if (choice == 0)
            {
                weapon = new LightWeapon("Dagger", '/', "A small quick blade.", 4, false);
            }
            else if (choice == 1)
            {
                weapon = new LightWeapon("Sword", '!', "A balanced one-handed sword.", 7, false);
            }
            else if (choice == 2)
            {
                weapon = new HeavyWeapon("Great Axe", '%', "A heavy two-handed axe.", 12, true);
            }
            else
            {
                weapon = new MagicalWeapon("Magic Staff", '?', "A staff filled with magical power.", 6, true);
            }

            return ApplyRandomModifiers(weapon, random);
        }

        private Item ApplyRandomModifiers(Item item, Random random)
        {
            bool addStrong = random.Next(2) == 0;
            bool addUnlucky = random.Next(2) == 0;

            if (addStrong)
            {
                item = new StrongModifier(item);
            }

            if (addUnlucky)
            {
                item = new UnluckyModifier(item);
            }

            return item;
        }
    }
}