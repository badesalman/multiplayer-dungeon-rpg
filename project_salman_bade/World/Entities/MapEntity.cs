using ConOOD_RpgGame.World.Entities.Core;

namespace ConOOD_RpgGame.World.Entities
{
    public abstract class MapEntity
    {
        public Position Position { get; protected set; }
        public abstract char Symbol { get; }

        protected MapEntity(Position position)
        {
            Position = position;
        }

        public void SetPosition(Position position)
        {
            Position = position;
        }
    }
}