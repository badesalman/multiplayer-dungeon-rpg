using ConOOD_RpgGame.World.Entities;
using ConOOD_RpgGame.World.Entities.Core;
using ConOOD_RpgGame.World.Entities.Core.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConOOD_RpgGame.World
{

    public class World
    {
        public const int Rows = 20;
        public const int Cols = 40;
        //boyut
        private readonly Cell[,] cells;

        public World()//const: grid oluşur her yer floor 
        {
            cells = new Cell[Rows, Cols];

            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Cols; col++)
                {
                    cells[row, col] = new Cell(new FloorTile());
                }
            }

        }

        public bool IsInside(Position position)
        {
            return position.Row >= 0 &&
                   position.Row < Rows &&
                   position.Col >= 0 &&
                   position.Col < Cols;
        }

        public bool CanEnter(Position position)
        {
            return GetCell(position).Ground.IsPassable;
        }

        public Cell GetCell(Position position)
        {
            return cells[position.Row, position.Col];
        }

        private void PlaceWall(int row, int col)
        {
            cells[row, col].Ground = new WallTile();
        }
        public void SetGround(Position position, Tile tile)
        {
            cells[position.Row, position.Col].Ground = tile;
        }

        public void AddItem(Position position, Item item)
        {
            cells[position.Row, position.Col].AddItem(item);
        }
        public void ClearAllItems()
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Cols; col++)
                {
                    cells[row, col].Items.Clear();
                }
            }
        }
        public void AddEnemy(Position position, Enemy enemy)
        {
            cells[position.Row, position.Col].Enemy = enemy;
            enemy.SetPosition(position);
        }

        public Enemy GetEnemyAt(Position position)
        {
            return cells[position.Row, position.Col].Enemy;
        }

        public void RemoveEnemy(Position position)
        {
            cells[position.Row, position.Col].Enemy = null;
        }

        public bool HasEnemyAt(Position position)
        {
            Enemy enemy = cells[position.Row, position.Col].Enemy;
            return enemy != null && enemy.IsAlive;
        }
        public bool HasAnyItemsOnGround()
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Cols; col++)
                {
                    if (cells[row, col].HasItems())
                    {
                        return true;
                    }
                }
            }

            return false;

        }

    }
}