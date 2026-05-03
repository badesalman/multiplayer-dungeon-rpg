using ConOOD_RpgGame.World.Entities.Core.Items.Builders;
using ConOOD_RpgGame.World.Entities.Core.Items.Builders.Dungeon;

namespace ConOOD_RpgGame.World.Entities.Core.Items.Themes
{
    public class TreasureTheme : IDungeonTheme
    {
        public void ConfigureDungeon(DungeonBuilder builder)
        {
          
        {
            IDungeonStrategy strategy = new DungeonGroundsStrategy();
            strategy.Configure(builder);

            builder.AddStep(new AddThemeItemsStep(
                15,
                new List<Func<Item>>
                {
            () => new JunkItem("Coin", '$', "A small coin."),
            () => new JunkItem("Gold", '$', "Shiny gold.")
                }
            ));

            builder.AddStep(new AddEnemiesStep(4, "Angry Briefcase"));
        }
    } 
        public string GetIntroMessage()
        {
            return "You feel an itch in your wallet.";
        }

        public string GetThemeName()
        {
            return "Treasure";
        }
        public void AddThemeContent(World world)
        {
            Random random = new Random();

            while (true)
            {
                int row = random.Next(World.Rows);
                int col = random.Next(World.Cols);

                Position pos = new Position(row, col);

                if (world.CanEnter(pos)) // boş ve yürünebilir
                {
                    Item artifact = new LightWeapon(
    "Lucky Coin Pouch",
    '$',
    "A magical two-handed pouch filled with lucky coins.",
    3,
    true
);

                    world.GetCell(pos).AddItem(artifact);
                    break;
                }
            }
        }
    }
}