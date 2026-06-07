using System.Collections.Generic;

namespace ConOOD_RpgGame.Network.DTO
{
    public class GameStateDto
    {
        public int Rows { get; set; }
        public int Cols { get; set; }
        public char[][] Map { get; set; }

        public string ThemeLine { get; set; } = "";
        public string Instructions { get; set; } = "";

        public List<PlayerDto> Players { get; set; } = new List<PlayerDto>();
        public List<string> RecentLogs { get; set; } = new List<string>();

        public List<string> InventoryLines { get; set; } = new List<string>();
        public string LeftHandText { get; set; } = "(empty)";
        public string RightHandText { get; set; } = "(empty)";
        public string StandingOnText { get; set; } = "Nothing";

        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Luck { get; set; }
        public int Aggression { get; set; }
        public int Wisdom { get; set; }
        public int Health { get; set; }
        public int Coins { get; set; }
        public int Gold { get; set; }

        public string Message { get; set; } = "";
        public string InventoryText { get; set; } = "";
    }
}