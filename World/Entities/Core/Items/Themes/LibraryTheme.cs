using ConOOD_RpgGame.World.Entities.Core.Items.Builders;
using ConOOD_RpgGame.World.Entities.Core.Items.Modifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConOOD_RpgGame.World.Entities.Core.Items;
using ConOOD_RpgGame.World.Entities.Core.Items.Builders.Dungeon;

namespace ConOOD_RpgGame.World.Entities.Core.Items.Themes
{
    public class LibraryTheme : IDungeonTheme
    {
        
        public void ConfigureDungeon(DungeonBuilder builder)
        {
            IDungeonStrategy strategy = new DungeonGroundsStrategy();
            strategy.Configure(builder);

            // Theme farkı burada
            builder.AddStep(new AddThemeItemsStep(
    8,
    new List<Func<Item>>
    {
        () => new JunkItem("Old Book", '?', "A dusty book full of forgotten knowledge."),
        () => new JunkItem("Wisdom Scroll", '?', "A scroll that improves wisdom."),
        () => new StrongModifier(
            new MagicalWeapon("Ancient Wand", '*', "A magical library wand.",6,false)
        )
    }
));
            builder.AddStep(new AddEnemiesStep(5, "Mage",'E'));
            builder.AddStep(new AddEnemiesStep(2, "Goblin",'g'));
            builder.AddStep(new AddEnemiesStep(2, "Skeleton",'s'));
            builder.AddStep(new AddSlotDemoItemsStep());
        }

        public string GetIntroMessage()
        {
            return "The smell of old books fills the air.";
        }

        public string GetThemeName()
        {
            return "Library";
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
                        "Black Wand",
                        'W',
                        "A unique artifact of the Library dungeon."
                    );

                    world.GetCell(pos).AddItem(artifact);
                    break;
                }
            }
        }
    }
}
