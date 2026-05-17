using ConOOD_RpgGame.World.Entities.Core;
using ConOOD_RpgGame.World.Entities.Core.combat.Events;
using ConOOD_RpgGame.World.Entities.Core.Logging;
using ConOOD_RpgGame.World.Entities.Core.Movement;
using ConOOD_RpgGame.World.Entities.Core.Navigation;
using ConOOD_RpgGame.World.Entities.Core.observers;
using ConOOD_RpgGame.World.Entities.Core.Species;

namespace ConOOD_RpgGame.World.Entities
{
    public class Enemy : Character,
        INoiseListener,
        IDeathListener
    {
        public override char Symbol { get; }

        public string Name { get; }

        

        private readonly IDeathReactionStrategy deathReactionStrategy;
        private readonly SpeciesGroup speciesGroup;
        private readonly DungeonNavigator navigator;
        private readonly IMovementBehavior movementBehavior;

        private readonly ConOOD_RpgGame.World.World world;

        public Enemy(
            Position position,
            int health,
            int attack,
            int armor,
            string name,
            char symbol,
            IDeathReactionStrategy deathReactionStrategy,
            SpeciesGroup speciesGroup,
            DungeonNavigator navigator,
            IMovementBehavior movementBehavior,
            ConOOD_RpgGame.World.World world)
            : base(position, health,armor, attack)
        {
            Armor = armor;

            Name = name;

           
            Symbol = symbol;
            this.deathReactionStrategy = deathReactionStrategy;

            this.speciesGroup = speciesGroup;

            this.navigator = navigator;

            this.movementBehavior = movementBehavior;

            this.world = world;
        }

        public void OnNoiseHeard(NoiseEvent noiseEvent)
        {
            int distance = navigator.GetDistance(
                world,
                Position,
                noiseEvent.SourcePosition,
                noiseEvent.Range);

            if (distance == -1)
            {
                return;
            }

            GameLogger.Instance?.Log(
                $"{Name} heard a noise from " +
                $"{noiseEvent.SourceName} at distance {distance}. " +
                $"Enemy position: ({Position.Row},{Position.Col})");
        }

        public void OnDeathReceived(DeathEvent deathEvent)
        {
            deathReactionStrategy.React(this);

            GameLogger.Instance?.Log(
                $"{Name} reacted to death of species member.");
        }

        public void BroadcastDeath()
        {
            speciesGroup.BroadcastDeath(
                new DeathEvent(
                    Position,
                    Name));
        }

        public void SubscribeToSpecies()
        {
            speciesGroup.Subscribe(this);
        }

        public void UnsubscribeFromSpecies()
        {
            speciesGroup.Unsubscribe(this);
        }

        public void MoveRandomly()
        {
            movementBehavior.Move(this, world);
        }
    }
}