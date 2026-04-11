using System;
using System.Collections.Generic;
using System.Linq;

namespace ConOOD_RpgGame.World.Entities.Core
{
    public struct Position
    {
        public int Row {  get; }
        public int Col { get; }

        public Position(int row, int col) //constructor, pozisyon oluşturuyor
        {
            Row = row; Col = col;
        }
        public Position Offset(int dRow, int dCol)
        {
            return new Position(Row + dRow, Col + dCol);
        }
    }
}
