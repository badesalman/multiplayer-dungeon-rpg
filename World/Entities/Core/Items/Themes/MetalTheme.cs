using ConOOD_RpgGame.World.Entities.Core.Items.Builders;
using ConOOD_RpgGame.World.Entities.Core.Items.Modifiers;
using ConOOD_RpgGame.World.Entities.Core.Items;
using ConOOD_RpgGame.World.Entities.Core.Items.Builders.Dungeon;

namespace ConOOD_RpgGame.World.Entities.Core.Items.Themes
{
    public class MetalTheme : IDungeonTheme
    {
        public void ConfigureDungeon(DungeonBuilder builder)
        {
            IDungeonStrategy strategy = new DungeonGroundsStrategy();
            strategy.Configure(builder);

            builder.AddStep(new AddThemeItemsStep(
    8,
    new List<Func<Item>>
    {
        () => new JunkItem("Metal Fragment", '*', "A sharp piece of old metal."),
        () => new JunkItem("Broken Gear", '*', "A broken mechanical part."),
        () => new StrongModifier(
            new HeavyWeapon("Iron Hammer", '*',"A heavy metal hammer.",6,false)
        )
    }
));
            builder.AddStep(new AddEnemiesStep(6, "Cleaning Robot", 'E'));
            builder.AddStep(new AddEnemiesStep(2, "Goblin", 'g'));
            builder.AddStep(new AddEnemiesStep(2, "Skeleton", 's'));
            builder.AddStep(new AddSlotDemoItemsStep());
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
