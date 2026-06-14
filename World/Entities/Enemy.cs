using ConOOD_RpgGame.World.Entities.Core;
using ConOOD_RpgGame.World.Entities.Core.combat.Events;
using ConOOD_RpgGame.World.Entities.Core.Logging;
using ConOOD_RpgGame.World.Entities.Core.Movement;
using ConOOD_RpgGame.World.Entities.Core.Navigation;
using ConOOD_RpgGame.World.Entities.Core.observers;
using ConOOD_RpgGame.World.Entities.Core.Species;
using System.Collections.Generic;

namespace ConOOD_RpgGame.World.Entities
{
    public class Enemy : Character, INoiseListener, IDeathListener
    {
        public override char Symbol { get; }
        public string Name { get; }

        private const int LineOfSightRange = 6;

        private readonly IDeathReactionStrategy deathReactionStrategy;
        private readonly SpeciesGroup speciesGroup;
        private readonly DungeonNavigator navigator;
        private readonly ConOOD_RpgGame.World.World world;

        private IMovementBehavior defaultMovementBehavior;
        private ISoundReactionBehavior soundReactionBehavior;
        private IPlayerReactionBehavior playerReactionBehavior;

        private Position? lastHeardSound;
        private bool changesBehaviourAfterAttack;
        private readonly int maxHealth;

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
            : base(position, health, armor, attack)
        {
            Name = name;
            Symbol = symbol;

            this.deathReactionStrategy = deathReactionStrategy;
            this.speciesGroup = speciesGroup;
            this.navigator = navigator;
            this.world = world;

            maxHealth = health;
            defaultMovementBehavior = movementBehavior;

            SetNeutralBehaviour();
        }

        public void SetAggressiveBehaviour()
        {
            soundReactionBehavior = new FollowSoundBehavior();
            playerReactionBehavior = new FollowPlayerBehavior();
            changesBehaviourAfterAttack = false;
        }

        public void SetCowardlyBehaviour()
        {
            soundReactionBehavior = new RunAwayFromSoundBehavior();
            playerReactionBehavior = new RunAwayFromPlayerBehavior();
            changesBehaviourAfterAttack = false;
        }

        public void SetNeutralBehaviour()
        {
            soundReactionBehavior = new IgnoreSoundBehavior();
            playerReactionBehavior = new IgnorePlayerBehavior();
            changesBehaviourAfterAttack = true;
        }

        public void OnAttacked()
        {
            if (!changesBehaviourAfterAttack)
            {
                return;
            }

            if (Health >= maxHealth / 2)
            {
                SetAggressiveBehaviour();
                GameLogger.Instance?.Log($"{Name} became aggressive after being attacked.");
            }
            else
            {
                SetCowardlyBehaviour();
                GameLogger.Instance?.Log($"{Name} became cowardly after being attacked.");
            }
        }

        public void TakeTurn(IEnumerable<Player> players)
        {
            Player visiblePlayer = FindNearestVisiblePlayer(players, world);

            if (visiblePlayer != null)
            {
                playerReactionBehavior.Move(this, world, players);
                return;
            }

            MoveBySoundOrDefault();
        }

        public void MoveBySoundOrDefault()
        {
            if (lastHeardSound.HasValue)
            {
                soundReactionBehavior.Move(this, world, lastHeardSound.Value);
                return;
            }

            MoveDefault();
        }

        public void MoveDefault()
        {
            defaultMovementBehavior.Move(this, world);
        }

        public void ClearSoundTarget()
        {
            lastHeardSound = null;
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

            lastHeardSound = noiseEvent.SourcePosition;

            GameLogger.Instance?.Log(
                $"{Name} heard a noise from {noiseEvent.SourceName} at distance {distance}. " +
                $"Enemy position: ({Position.Row},{Position.Col})");
        }

        public Player FindNearestVisiblePlayer(IEnumerable<Player> players, ConOOD_RpgGame.World.World world)
        {
            Player nearest = null;
            int nearestDistance = 999999;

            foreach (Player player in players)
            {
                if (!player.IsAlive)
                {
                    continue;
                }

                int distance = GetLineOfSightDistance(player.Position, world);

                if (distance == -1)
                {
                    continue;
                }

                if (distance < nearestDistance)
                {
                    nearest = player;
                    nearestDistance = distance;
                }
            }

            return nearest;
        }

        private int GetLineOfSightDistance(Position target, ConOOD_RpgGame.World.World world)
        {
            if (Position.Row == target.Row)
            {
                return CheckHorizontalSight(target, world);
            }

            if (Position.Col == target.Col)
            {
                return CheckVerticalSight(target, world);
            }

            return -1;
        }

        private int CheckHorizontalSight(Position target, ConOOD_RpgGame.World.World world)
        {
            int direction = target.Col > Position.Col ? 1 : -1;
            int distance = 0;
            int col = Position.Col + direction;

            while (col != target.Col + direction && distance <= LineOfSightRange)
            {
                Position current = new Position(Position.Row, col);

                if (!world.IsInside(current))
                {
                    return -1;
                }

                if (!world.GetTile(current.Row, current.Col).IsPassable)
                {
                    return -1;
                }

                if (current.Row == target.Row && current.Col == target.Col)
                {
                    return distance + 1;
                }

                col += direction;
                distance++;
            }

            return -1;
        }

        private int CheckVerticalSight(Position target, ConOOD_RpgGame.World.World world)
        {
            int direction = target.Row > Position.Row ? 1 : -1;
            int distance = 0;
            int row = Position.Row + direction;

            while (row != target.Row + direction && distance <= LineOfSightRange)
            {
                Position current = new Position(row, Position.Col);

                if (!world.IsInside(current))
                {
                    return -1;
                }

                if (!world.GetTile(current.Row, current.Col).IsPassable)
                {
                    return -1;
                }

                if (current.Row == target.Row && current.Col == target.Col)
                {
                    return distance + 1;
                }

                row += direction;
                distance++;
            }

            return -1;
        }

        public void OnDeathReceived(DeathEvent deathEvent)
        {
            deathReactionStrategy.React(this);

            GameLogger.Instance?.Log($"{Name} reacted to death of species member.");
        }

        public void BroadcastDeath()
        {
            speciesGroup.BroadcastDeath(new DeathEvent(Position, Name));
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
            MoveDefault();
        }
    }
}