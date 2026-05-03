using ConOOD_RpgGame.World.Entities.Core.Items.Builders;
using ConOOD_RpgGame.World.Entities.Core.Items.Builders.Dungeon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConOOD_RpgGame.World.Entities.Core.Items.Themes
{
    public class LibraryTheme : IDungeonTheme
    {
        
        public void ConfigureDungeon(DungeonBuilder builder)
        {
            IDungeonStrategy strategy = new DungeonGroundsStrategy();
            strategy.Configure(builder);

            // Theme farkı burada
            builder.AddStep(new AddItemsStep(12));
            builder.AddStep(new AddWeaponsStep(3));
            builder.AddStep(new AddEnemiesStep(5, "Mage"));
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
