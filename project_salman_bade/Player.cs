using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConOOD_RpgGame
{
    public class Player
    {
        public Position Position { get; private set; } 

        public int Strength { get; }
        public int Dexterity { get; }
        public int Health { get; }
        public int Luck { get; }
        public int Aggression { get; }
        public int Wisdom { get; }
        
        public List<Item> Inventory { get; } //inventoryde bulunanlar 

        public Item LeftHandItem { get; private set; }
        public Item RightHandItem { get; private set; }
       
        public int Coins { get; private set; }
        public int Gold { get; private set; }

        public Player(Position startPosition)
        {
            Position = startPosition;

            Strength = 10;
            Dexterity = 8;
            Health = 20;
            Luck = 6;
            Aggression = 7;
            Wisdom = 9;

            Inventory = new List<Item>(); 
        }

        public void AddCoins(int amount)
        {
            Coins += amount;
        }

        public void AddGold(int amount)
        {
            Gold += amount;
        }

        public void AddToInventory(Item item)
        {
            Inventory.Add(item);
        }
        //worldden geliyo canenter ispassable etc.
        public bool Move(World world, int dRow, int dCol, out string message)
        {
            Position newPosition = Position.Offset(dRow, dCol);

            if (!world.IsInside(newPosition))
            {
                message = "You cannot move outside the map.";
                return false;
            }

            if (!world.CanEnter(newPosition))
            {
                message = "A wall blocks your way.";
                return false;
            }

            Position = newPosition;
            message = "Moved.";
            return true;
        }

        public bool PickUpTopItem(World world, out string message) //oyuncunun bulunduğu hücre alınır item var mı bakılır 
            // üstteki item alınır ve item kendi pickup davranışını uygular
        {
            Cell cell = world.GetCell(Position);

            if (!cell.HasItems())
            {
                message = "There is nothing here.";
                return false;
            }

            Item item = cell.TakeTopItem();
            bool success = item.PickUp(this, out message);

            if (!success)
            {
                cell.AddItem(item);//yere bırakır 
            }

            return success;
        }

        public bool DropItem(World world, int inventoryIndex, out string message)
        {
            if (inventoryIndex < 0 || inventoryIndex >= Inventory.Count)
            {
                message = "Invalid inventory index.";
                return false;
            }

            Item item = Inventory[inventoryIndex]; //first it drops from incentory

            UnequipItemIfNeeded(item);

            Inventory.RemoveAt(inventoryIndex);
            world.GetCell(Position).AddItem(item);

            message = $"{item.Name} dropped on the ground.";
            return true;
        }

        public bool EquipItem(int inventoryIndex, HandSide handSide, out string message) //karar itemin kendisine bıırakılır junitem equip olamaz coin olamaz ama 
            //weapon olabilir
        {
            if (inventoryIndex < 0 || inventoryIndex >= Inventory.Count)
            {
                message = "Invalid inventory index.";
                return false;
            }

            Item item = Inventory[inventoryIndex];
            return item.TryEquip(this, handSide, out message);
        }

        public bool EquipWeapon(Weapon weapon, HandSide handSide, out string message)
        {
            if (!Inventory.Contains(weapon))
            {
                message = "You can only equip a weapon from your inventory.";
                return false;
            }

            // Aynı item zaten bir elde takılıysa, tekrar takılamaz
            if (LeftHandItem == weapon || RightHandItem == weapon)
            {
                message = "This weapon is already equipped.";
                return false;
            }

            if (weapon.IsTwoHanded)
            {
                if (LeftHandItem != null || RightHandItem != null)
                {
                    message = "Both hands must be empty to equip a two-handed weapon.";
                    return false;
                }

                LeftHandItem = weapon;
                RightHandItem = weapon;
                message = $"{weapon.Name} equipped in both hands.";
                return true;
            }

            if (HasTwoHandedWeaponEquipped())//zaten two handedn silah var başka takamaz 
            {
                message = "Unequip the two-handed weapon first.";
                return false;
            }

            if (handSide == HandSide.Left)
            {
                if (LeftHandItem != null)
                {
                    message = "Left hand is already occupied.";
                    return false;
                }

                LeftHandItem = weapon;
                message = $"{weapon.Name} equipped in left hand.";
                return true;
            }

            if (RightHandItem != null)
            {
                message = "Right hand is already occupied.";
                return false;
            }

            RightHandItem = weapon;
            message = $"{weapon.Name} equipped in right hand.";
            return true;


        }

        public bool Unequip(HandSide handSide, out string message)
        {
            if (handSide == HandSide.Left)
            {
                if (LeftHandItem == null)
                {
                    message = "Left hand is already empty.";
                    return false;
                }

                if (LeftHandItem == RightHandItem)
                {
                    string weaponName = LeftHandItem.Name;
                    LeftHandItem = null;
                    RightHandItem = null;
                    message = $"{weaponName} unequipped from both hands.";
                    return true;
                }

                string leftName = LeftHandItem.Name;
                LeftHandItem = null;
                message = $"{leftName} unequipped from left hand.";
                return true;
            }

            if (RightHandItem == null)
            {
                message = "Right hand is already empty.";
                return false;
            }

            if (LeftHandItem == RightHandItem)
            {
                string weaponName = RightHandItem.Name;
                LeftHandItem = null;
                RightHandItem = null;
                message = $"{weaponName} unequipped from both hands.";
                return true;
            }

            string rightName = RightHandItem.Name;
            RightHandItem = null;
            message = $"{rightName} unequipped from right hand.";
            return true;
        }

        private bool HasTwoHandedWeaponEquipped() //two handed logic eğer iki elde aynı item varsa 
        {
            return LeftHandItem != null && LeftHandItem == RightHandItem;
        }

        private void UnequipItemIfNeeded(Item item) //helper met drop yapılınca elleri temizler
        {
            if (LeftHandItem == item && RightHandItem == item)
            {
                LeftHandItem = null;
                RightHandItem = null;
                return;
            }

            if (LeftHandItem == item)
            {
                LeftHandItem = null;
            }
            if (RightHandItem == item)
            {
                RightHandItem = null;
            }
        }
    }

}
