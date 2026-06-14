using System;
using System.Collections.Generic;
using ConOOD_RpgGame.World.Entities.Core.Items;

namespace ConOOD_RpgGame.World.Entities.Core.Items.Builders
{
    public class AddSlotDemoItemsStep : IBuildStep
    {
        public void Apply(World world, Random random)
        {
            Weapon sword = new LightWeapon(
                "Slotted Sword",
                '!',
                "A sword with two slots.",
                2,
                false,
                2);

            PassiveItem luckGem = new PassiveItem(
                "Luck Gem",
                '+',
                "A gem granting luck.",
                0,
                0,
                2,
                0,
                0);

            PassiveItem strengthGem = new PassiveItem(
                "Strength Gem",
                '+',
                "A gem granting strength.",
                2,
                0,
                0,
                0,
                0);

            PassiveItem wisdomGem = new PassiveItem(
                "Wisdom Gem",
                '+',
                "A gem granting wisdom.",
                0,
                0,
                0,
                0,
                2);

            HolderItem smallHolder = new HolderItem(
                "Small Holder",
                'h',
                "A holder with two slots.",
                2);

            HolderItem deepHolder = new HolderItem(
                "Deep Holder",
                'h',
                "Another holder with two slots.",
                2);

            deepHolder.AddToSlot(wisdomGem, out string _);
            smallHolder.AddToSlot(strengthGem, out string _);
            smallHolder.AddToSlot(deepHolder, out string _);

            sword.AddToSlot(luckGem, out string _);
            sword.AddToSlot(smallHolder, out string _);

            AddItemToRandomPosition(world, random, sword);

            AddItemToRandomPosition(
                world,
                random,
                new HolderItem("Empty Holder", 'h', "An empty holder on the map.", 2));

            AddItemToRandomPosition(
                world,
                random,
                new HolderItem("Second Empty Holder", 'h', "Another empty holder on the map.", 2));
        }

        private void AddItemToRandomPosition(World world, Random random, Item item)
        {
            List<Position> positions = new List<Position>();

            for (int row = 0; row < World.Rows; row++)
            {
                for (int col = 0; col < World.Cols; col++)
                {
                    Position position = new Position(row, col);

                    if (world.CanEnter(position))
                    {
                        positions.Add(position);
                    }
                }
            }

            if (positions.Count == 0)
            {
                return;
            }

            Position selected = positions[random.Next(positions.Count)];
            world.AddItem(selected, item);
        }
    }
}