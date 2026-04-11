namespace ConOOD_RpgGame.World.Entities
{
    public interface IAttackable
    {
        int Health { get; }
        int Armor { get; }
        bool IsAlive { get; }

        void ReceiveDamage(int damage);
    }
}