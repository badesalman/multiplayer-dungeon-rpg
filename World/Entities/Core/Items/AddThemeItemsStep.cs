using ConOOD_RpgGame.World.Entities.Core.Items.Builders;
using System;
using System.Collections.Generic;

namespace ConOOD_RpgGame.World.Entities.Core.Items
{
    public class AddThemeItemsStep : IBuildStep
    {
        private readonly int count;
        private readonly List<Func<Item>> itemFactories;

        public AddThemeItemsStep(int count, List<Func<Item>> itemFactories)
        {
            this.count = count;
            this.itemFactories = itemFactories;
        }

        public void Apply(World world, Random random)
        {
            for (int i = 0; i < count; i++)
            {
                Position pos = FindRandomPassablePosition(world, random);

                int index = random.Next(itemFactories.Count);
                Item item = itemFactories[index]();

                world.GetCell(pos).AddItem(item);
            }
        }

        private Position FindRandomPassablePosition(World world, Random random)
        {
            while (true)
            {
                Position pos = new Position(
                    random.Next(World.Rows),
                    random.Next(World.Cols)
                );

                if (world.CanEnter(pos))
                {
                    return pos;
                }
            }
        }
    }
}