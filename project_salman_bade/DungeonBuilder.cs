using System;
using System.Collections.Generic;

namespace ConOOD_RpgGame
{
    public class DungeonBuilder
    {
        private readonly List<IBuildStep> steps;
        private bool hasStarterStep;

        public DungeonBuilder()
        {
            steps = new List<IBuildStep>();
            hasStarterStep = false;
        }

        public DungeonBuilder StartWithEmptyDungeon()
        {
            EnsureStarterNotChosen();
            steps.Add(new EmptyDungeonStep());
            hasStarterStep = true;
            return this;
        }

        public DungeonBuilder StartWithFilledDungeon()
        {
            EnsureStarterNotChosen();
            steps.Add(new FilledDungeonStep());
            hasStarterStep = true;
            return this;
        }

        public DungeonBuilder AddStep(IBuildStep step)
        {
            if (!hasStarterStep)
            {
                throw new InvalidOperationException("A starter step must be added first.");
            }

            steps.Add(step);
            return this;
        }

        public void Build(World world)
        {
            if (!hasStarterStep)
            {
                throw new InvalidOperationException("Cannot build dungeon without a starter step.");
            }

            world.ClearAllItems();

            Random random = new Random();

            foreach (IBuildStep step in steps)
            {
                step.Apply(world, random);
            }
        }

        private void EnsureStarterNotChosen()
        {
            if (hasStarterStep)
            {
                throw new InvalidOperationException("Only one starter step can be used.");
            }
        }
    }
}

