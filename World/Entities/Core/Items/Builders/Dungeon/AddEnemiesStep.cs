using ConOOD_RpgGame.World.Entities.Core.Logging;
using ConOOD_RpgGame.World.Entities.Core.Movement;
using ConOOD_RpgGame.World.Entities.Core.Navigation;
using ConOOD_RpgGame.World.Entities.Core.Species;
using System;
using System.Collections.Generic;

namespace ConOOD_RpgGame.World.Entities.Core.Items.Builders.Dungeon
{
    public class AddEnemiesStep : IBuildStep
    {
        private readonly int enemyCount;
        private readonly string enemyName;
        private readonly char symbol;
        private readonly SpeciesGroup speciesGroup;
        private readonly IDeathReactionStrategy deathReactionStrategy;
        private readonly DungeonNavigator navigator;
        private readonly IMovementBehavior movementBehavior;
        private readonly SoundManager soundManager;

        public AddEnemiesStep(
            Player player,
            int enemyCount,
            string enemyName,
            char symbol,
            SpeciesGroup speciesGroup,
            IDeathReactionStrategy deathReactionStrategy,
            DungeonNavigator navigator,
            IMovementBehavior movementBehavior,
            SoundManager soundManager)
        {
            this.enemyCount = enemyCount;
            this.enemyName = enemyName;
            this.symbol = symbol;
            this.speciesGroup = speciesGroup;
            this.deathReactionStrategy = deathReactionStrategy;
            this.navigator = navigator;
            this.movementBehavior = movementBehavior;
            this.soundManager = soundManager;
        }

        public AddEnemiesStep(int enemyCount, string enemyName, char symbol)
        {
            this.enemyCount = enemyCount;
            this.enemyName = enemyName;
            this.symbol = symbol;

            speciesGroup = new SpeciesGroup(enemyName);
            deathReactionStrategy = new GoblinDeathReactionStrategy();
            navigator = new DungeonNavigator();
            movementBehavior = new RandomMovementBehavior();
            soundManager = new SoundManager();
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

                Enemy enemy = CreateRandomEnemy(pos, random, world);
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

                    if (world.CanEnter(pos) && !world.HasEnemyAt(pos))
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

        private Enemy CreateRandomEnemy(Position position, Random random, World world)
        {
            int health = random.Next(30, 51);
            int attack = random.Next(4, 7);
            int armor = random.Next(2, 5);

            Enemy enemy = new Enemy(
                position,
                health,
                attack,
                armor,
                enemyName,
                symbol,
                deathReactionStrategy,
                speciesGroup,
                navigator,
                movementBehavior,
                world);
            ConfigureStage7Behaviour(enemy);

            enemy.SubscribeToSpecies();
            soundManager.Subscribe(enemy);

            GameLogger.Instance.Log($"{enemy.Name} subscribed to sound manager.");

            return enemy;
     
        }
        private void ConfigureStage7Behaviour(Enemy enemy)
        {
            if (enemyName == "Skeleton")
            {
                enemy.SetAggressiveBehaviour();
                return;
            }

            if (enemyName == "Goblin")
            {
                enemy.SetCowardlyBehaviour();
                return;
            }

            enemy.SetNeutralBehaviour();
        }
    }
}