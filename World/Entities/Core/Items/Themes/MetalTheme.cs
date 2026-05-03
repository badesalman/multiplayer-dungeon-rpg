using ConOOD_RpgGame.World.Entities.Core.Items.Builders;
using ConOOD_RpgGame.World.Entities.Core.Items.Builders.Dungeon;

namespace ConOOD_RpgGame.World.Entities.Core.Items.Themes
{
    public class MetalTheme : IDungeonTheme
    {
        public void ConfigureDungeon(DungeonBuilder builder)
        {
            IDungeonStrategy strategy = new DungeonGroundsStrategy();
            strategy.Configure(builder);

            builder.AddStep(new AddWeaponsStep(8));
            builder.AddStep(new AddEnemiesStep(6, "Cleaning Robot"));
        }
        public string GetIntroMessage()
        {
            return "The clang of metal echoes off the walls.";
        }

        public string GetThemeName()
        {
            return "Metal";
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
                       "Blaster",
                       'B',
                       "A unique artifact of the Metal dungeon."
                    );

                    world.GetCell(pos).AddItem(artifact);
                    break;
                }
            }
        }
    }
}
//interface 2 method istiyor
