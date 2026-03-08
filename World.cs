using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConOOD_RpgGame
{

        public class World
        {
            public const int Rows = 20;
            public const int Cols = 40;

            private readonly Cell[,] cells;

            public World()
            {
                cells = new Cell[Rows, Cols];

                for (int row = 0; row < Rows; row++)
                {
                    for (int col = 0; col < Cols; col++)
                    {
                        cells[row, col] = new Cell(new FloorTile());
                    }
                }

                InitializeWalls();
                InitializeItems();
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

            private void PlaceItem(int row, int col, Item item)
            {
                cells[row, col].AddItem(item);
            }

            private void InitializeWalls()
            {
                for (int row = 2; row < 10; row++)
                {
                    PlaceWall(row, 10);
                }

                for (int col = 15; col < 30; col++)
                {
                    PlaceWall(12, col);
                }

                for (int row = 5; row < 15; row++)
                {
                    PlaceWall(row, 25);
                }

                PlaceWall(1, 5);
                PlaceWall(1, 6);
                PlaceWall(1, 7);
                PlaceWall(16, 35);
                PlaceWall(17, 35);
                PlaceWall(18, 35);
            }

            private void InitializeItems()
            {
                PlaceItem(0, 1, new Weapon("Dagger", '/', "A small quick blade.", 4, false));
                PlaceItem(2, 2, new Weapon("Sword", '!', "A balanced one-handed sword.", 7, false));
                PlaceItem(10, 4, new Weapon("Great Axe", '%', "A heavy two-handed axe.", 12, true));

                PlaceItem(3, 8, new JunkItem("Old Boot", '?', "A worn-out boot."));
                PlaceItem(6, 3, new JunkItem("Broken Key", '?', "It looks useless now."));
                PlaceItem(8, 12, new JunkItem("Cracked Orb", '?', "A strange but unusable item."));

                PlaceItem(1, 2, new Coin(5));
                PlaceItem(7, 7, new Coin(10));
                PlaceItem(10, 5, new Gold(2));
                PlaceItem(14, 20, new Gold(1));

                PlaceItem(4, 4, new Coin(3));
                PlaceItem(4, 4, new JunkItem("Dusty Note", '?', "The text is unreadable."));
            }
        }
    }
