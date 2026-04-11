using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConOOD_RpgGame.World.Entities
{
    public abstract class Tile
    {
        public abstract char Symbol { get; } // bu o tile ekranda hangi karakterle çizileceğini söyler 
        public abstract bool IsPassable{ get; }// bu oyuncu o tiledan geçer mi geçmez mi onu söyler 
    }
    public class FloorTile : Tile
    {// boş zemin ekranda boş görünür oyuncu üstüne basabilir
        public override char Symbol => ' ';
        public override bool IsPassable => true;
    }
    public class WallTile : Tile
    {
        // ekranda blok gibi görünür oyuncu içinden geçemez
        public override char Symbol => '█';
        public override bool IsPassable => false;
    }
}

//IsPassable geçer mi geçmez mi kontrolü