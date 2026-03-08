
using System.Collections.Generic;

namespace ConOOD_RpgGame
{
    public class Cell
    {
        public Tile Ground { get; set; }
        public List<Item> Items { get; }

        public Cell(Tile ground)
        {
            Ground = ground;
            Items = new List<Item>();
        }

        public bool HasItems()
        {
            return Items.Count > 0;
        }

        public Item PeekTopItem()
        {
            if (Items.Count == 0)
            {
                return null;
            }

            return Items[Items.Count - 1];
        }

        public Item TakeTopItem()
        {
            if (Items.Count == 0)
            {
                return null;
            }

            Item item = Items[Items.Count - 1];
            Items.RemoveAt(Items.Count - 1);
            return item;
        }

        public void AddItem(Item item)
        {
            Items.Add(item);
        }
    }
}