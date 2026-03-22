
using System.Collections.Generic;

namespace ConOOD_RpgGame
{
    public class Cell // bit harita hücresi
    {
        public Tile Ground { get; set; } //yer
        public List<Item> Items { get; } // liste çünkü coin weapon junk item aynı anda olabilir

        public Cell(Tile ground)//cons
        {
            Ground = ground; //yer olacak
            Items = new List<Item>();// item boş
        }

        public bool HasItems()
        {
            return Items.Count > 0; //looks for if we have item
        }

        public Item PeekTopItem() // shows item on floor
        {
            if (Items.Count == 0)
            {
                return null;
            }

            return Items[Items.Count - 1];
        }

        public Item TakeTopItem() //E
        {
            if (Items.Count == 0)
            {
                return null;
            }

            Item item = Items[Items.Count - 1];
            Items.RemoveAt(Items.Count - 1);
            return item;
        }

        public void AddItem(Item item)//hücreye item ekler 
        {
            Items.Add(item);
        }
    }
}