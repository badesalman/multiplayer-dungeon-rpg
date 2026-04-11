using ConOOD_RpgGame.World.Entities;

namespace ConOOD_RpgGame.World.Entities.Core.Items
{
    public class Coin : Item
    {
        public int Amount { get; }

        public Coin(int amount)
            : base("Coins", '$', "Regular coins")
        {
            Amount = amount; // kaç coin olduğunu tutar
        }

        public override string GetDisplayText()
        {
            return $"{Name} x{Amount}";//incentoryde nasıl görünecek
        }

        public override string GetDetailedInfo()// detaylı bilgi
        {
            return $"{Name} x{Amount} - {Description}";
        }

        public override bool PickUp(Player player, out string message)
        {// adds to inventory
            player.AddCoins(Amount);
            message = $"Picked up {Amount} coins.";
            return true;
        }
    }

    public class Gold : Item
    {
        public int Amount { get; }

        public Gold(int amount)
            : base("Gold", 'G', "Precious gold") //holds how much 
        {
            Amount = amount;
        }

        public override string GetDisplayText()
        {
            return $"{Name} x{Amount}";
        }

        public override string GetDetailedInfo()
        {
            return $"{Name} x{Amount} - {Description}";
        }

        public override bool PickUp(Player player, out string message)
        {
            player.AddGold(Amount);
            message = $"Picked up {Amount} gold.";
            return true;
        }
    }
}
