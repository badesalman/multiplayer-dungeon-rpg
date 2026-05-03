using ConOOD_RpgGame.World.Entities.Core.Items.Builders;
using ConOOD_RpgGame.World.Entities.Core.Items.Builders.Dungeon;

namespace ConOOD_RpgGame.World.Entities.Core.Items.Themes
{
    public class TreasureTheme : IDungeonTheme
    {
        public void ConfigureDungeon(DungeonBuilder builder)
        {
            IDungeonStrategy strategy = new DungeonGroundsStrategy();
            strategy.Configure(builder);

            builder.AddStep(new AddItemsStep(20)); // çok gold
            builder.AddStep(new AddEnemiesStep(4, "Living Safe"));
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
                    var artifact = new ArtifactItem(
                       "Lucky Coin Pouch",
                       '$', 
                       "A unique artifact of the Treasure dungeon."
                    );

                    world.GetCell(pos).AddItem(artifact);
                    break;
                }
            }
        }
    }
}