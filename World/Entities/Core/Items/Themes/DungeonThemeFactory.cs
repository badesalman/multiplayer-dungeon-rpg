using ConOOD_RpgGame.World.Entities.Core.Items.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConOOD_RpgGame.World.Entities.Core.Items.Themes
{
    public static class DungeonThemeFactory
    {
        private static Random random = new Random(Guid.NewGuid().GetHashCode());

        public static IDungeonTheme CreateRandomTheme()
        {
            int choice = random.Next(3);

            return choice switch
            {
                0 => new LibraryTheme(),
                1 => new MetalTheme(),
                _ => new TreasureTheme()
            };
        }
    }
}
