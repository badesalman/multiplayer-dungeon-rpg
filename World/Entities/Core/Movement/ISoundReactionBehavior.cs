using ConOOD_RpgGame.World.Entities;

namespace ConOOD_RpgGame.World.Entities.Core.Movement
{
    public interface ISoundReactionBehavior
    {
        void Move(Enemy enemy, World world, Position soundPosition);
    }
}