
using System.Numerics;

namespace ConOOD_RpgGame
{
    public class Weapon : Item //weapon is an item
    {
        public int Damage { get; }
        public bool IsTwoHanded { get; }

        public Weapon(string name, char symbol, string description, int damage, bool isTwoHanded)//cons
            : base(name, symbol, description) //item cons çağırılıyor
        {
            Damage = damage;
            IsTwoHanded = isTwoHanded;//parametreleri proppertylere ayırıyoruz
        }

        public override string GetDisplayText()
        {
            string handText = IsTwoHanded ? "Two-Handed" : "One-Handed";//ternary operator
            return $"{Name} (DMG: {Damage}, {handText})";
        }

        public override string GetDetailedInfo()
        {
            string handText = IsTwoHanded ? "Two-Handed" : "One-Handed";
            return $"{Name} - Damage: {Damage}, {handText}, {Description}";
        }

        public override bool TryEquip(Player player, HandSide handSide, out string message)
        {
            return player.EquipWeapon(this, handSide, out message);
        }
    }
}//silah kendisini equip etmiyor playera soruyor el boş mu two-handed mi iki el dolu mu
// if(item is weapon) yerine polymorphism