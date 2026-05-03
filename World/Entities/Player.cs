using ConOOD_RpgGame.World.Entities.Core;
using ConOOD_RpgGame.World.Entities.Core.Items;
using ConOOD_RpgGame.World.Enums;
using System.Collections.Generic;

namespace ConOOD_RpgGame.World.Entities
{
    public class Player : Character
    {
        public override char Symbol => '¶';

        public int BaseStrength { get; }
        public int BaseDexterity { get; }
        public int BaseLuck { get; }
        public int BaseAggression { get; }
        public int BaseWisdom { get; }

        public List<Item> Inventory { get; }

        public Item LeftHandItem { get; private set; }
        public Item RightHandItem { get; private set; }

        public int Coins { get; private set; }
        public int Gold { get; private set; }

        public Player(Position startPosition)
            : base(startPosition, 20, 0)
        {
            BaseStrength = 10;
            BaseDexterity = 8;
            BaseLuck = 6;
            BaseAggression = 7;
            BaseWisdom = 9;

            Inventory = new List<Item>();
        }

        // STAT GETTERS (modifier destekli)
         public int GetStrength()
        {
            return BaseStrength + GetEquippedItemBonus(item => item.GetStrengthBonus());
        }

        public int GetDexterity()
        {
            return BaseDexterity + GetEquippedItemBonus(item => item.GetDexterityBonus());
        }

        public int GetLuck()
        {
            return BaseLuck + GetEquippedItemBonus(item => item.GetLuckBonus());
        }

        public int GetAggression()
        {
            return BaseAggression + GetEquippedItemBonus(item => item.GetAggressionBonus());
        }

        public int GetWisdom()
        {
            return BaseWisdom + GetEquippedItemBonus(item => item.GetWisdomBonus());
        }

        public int GetCurrentHealth()
        {
            return Health;
        }
        public Item GetEquippedCombatItem()
{
    if (RightHandItem != null)
        return RightHandItem;

    if (LeftHandItem != null)
        return LeftHandItem;

    return null;
}

public int GetCurrentWeaponDamageBonus()
{
    Item item = GetEquippedCombatItem();

    if (item == null)
        return 0;

    return item.GetDamageBonus();
}
        public bool IsDead()
        {
            return !IsAlive;
        }

        private int GetEquippedItemBonus(System.Func<Item, int> selector)
        {
            int total = 0;

            if (LeftHandItem != null)
                total += selector(LeftHandItem);

            if (RightHandItem != null && RightHandItem != LeftHandItem)
                total += selector(RightHandItem);

            return total;
        }

        // ECONOMY
       
        public void AddCoins(int amount)
        {
            Coins += amount;
        }

        public void AddGold(int amount)
        {
            Gold += amount;
        }

        // INVENTORY
        

        public void AddToInventory(Item item)
        {
            Inventory.Add(item);
        }

        public bool PickUpTopItem(World world, out string message)
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
                cell.AddItem(item);

            return success;
        }

        public bool DropItem(World world, int index, out string message)
        {
            if (index < 0 || index >= Inventory.Count)
            {
                message = "Invalid inventory index.";
                return false;
            }

            Item item = Inventory[index];

            UnequipItemIfNeeded(item);

            Inventory.RemoveAt(index);
            world.GetCell(Position).AddItem(item);

            message = $"{item.Name} dropped.";
            return true;
        }

        // EQUIP

        public bool EquipItemDirectly(Item item, HandSide side, out string message)
        {

            {
                if (LeftHandItem == item || RightHandItem == item)
                {
                    message = "Already equipped.";
                    return false;
                }

                if (!item.CanBeEquipped())
                {
                    message = "Item is not a weapon.";
                    return false;
                }

                if (item.IsTwoHandedWeapon())
                {
                    if (LeftHandItem != null || RightHandItem != null)
                    {
                        message = "Both hands must be empty.";
                        return false;
                    }

                    LeftHandItem = item;
                    RightHandItem = item;
                    message = $"{item.GetDisplayText()} equipped (two-handed).";
                    return true;
                }

                if (HasTwoHandedWeaponEquipped())
                {
                    message = "Unequip two-handed weapon first.";
                    return false;
                }

                if (side == HandSide.Left)
                {
                    if (LeftHandItem != null)
                    {
                        message = "Left hand occupied.";
                        return false;
                    }

                    LeftHandItem = item;
                    message = $"{item.GetDisplayText()} equipped (left).";
                    return true;
                }

                if (RightHandItem != null)
                {
                    message = "Right hand occupied.";
                    return false;
                }

                RightHandItem = item;
                message = $"{item.GetDisplayText()} equipped (right).";
                return true;
            }
        }
        
        public bool EquipItem(int index, HandSide side, out string message)
        {
            if (index < 0 || index >= Inventory.Count)
            {
                message = "Invalid index.";
                return false;
            }

            Item item = Inventory[index];
            return item.TryEquip(this, side, out message);
        }

        public bool EquipWeapon(Weapon weapon, HandSide side, out string message)
        {
            if (!Inventory.Contains(weapon))
            {
                message = "Weapon must be in inventory.";
                return false;
            }

            if (LeftHandItem == weapon || RightHandItem == weapon)
            {
                message = "Already equipped.";
                return false;
            }

            if (weapon.IsTwoHanded)
            {
                if (LeftHandItem != null || RightHandItem != null)
                {
                    message = "Both hands must be empty.";
                    return false;
                }

                LeftHandItem = weapon;
                RightHandItem = weapon;
                message = $"{weapon.Name} equipped (two-handed).";
                return true;
            }

            if (HasTwoHandedWeaponEquipped())
            {
                message = "Unequip two-handed weapon first.";
                return false;
            }

            if (side == HandSide.Left)
            {
                if (LeftHandItem != null)
                {
                    message = "Left hand occupied.";
                    return false;
                }

                LeftHandItem = weapon;
                message = $"{weapon.Name} equipped (left).";
                return true;
            }

            if (RightHandItem != null)
            {
                message = "Right hand occupied.";
                return false;
            }

            RightHandItem = weapon;
            message = $"{weapon.Name} equipped (right).";
            return true;
        }

        public bool Unequip(HandSide side, out string message)
        {
            if (side == HandSide.Left)
            {
                if (LeftHandItem == null)
                {
                    message = "Left empty.";
                    return false;
                }

                if (LeftHandItem == RightHandItem)
                {
                    string name = LeftHandItem.Name;
                    LeftHandItem = null;
                    RightHandItem = null;
                    message = $"{name} unequipped.";
                    return true;
                }

                string left = LeftHandItem.Name;
                LeftHandItem = null;
                message = $"{left} removed.";
                return true;
            }

            if (RightHandItem == null)
            {
                message = "Right empty.";
                return false;
            }

            if (LeftHandItem == RightHandItem)
            {
                string name = RightHandItem.Name;
                LeftHandItem = null;
                RightHandItem = null;
                message = $"{name} unequipped.";
                return true;
            }

            string right = RightHandItem.Name;
            RightHandItem = null;
            message = $"{right} removed.";
            return true;
        }

        private bool HasTwoHandedWeaponEquipped()
        {
            return LeftHandItem != null && LeftHandItem == RightHandItem;
        }

        private void UnequipItemIfNeeded(Item item)
        {
            if (LeftHandItem == item && RightHandItem == item)
            {
                LeftHandItem = null;
                RightHandItem = null;
                return;
            }

            if (LeftHandItem == item)
                LeftHandItem = null;

            if (RightHandItem == item)
                RightHandItem = null;
        }

        // MOVEMENT
        public bool Move(World world, int dRow, int dCol, out string message)
        {
            Position newPos = Position.Offset(dRow, dCol);

            if (!world.IsInside(newPos))
            {
                message = "Out of bounds.";
                return false;
            }

            if (!world.CanEnter(newPos))
            {
                message = "Blocked.";
                return false;
            }

            Position = newPos;
            message = "Moved.";
            return true;
        }
    }
}