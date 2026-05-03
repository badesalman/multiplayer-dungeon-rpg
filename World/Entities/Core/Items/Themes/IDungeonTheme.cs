using ConOOD_RpgGame.World.Entities.Core.Items.Builders.Dungeon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConOOD_RpgGame.World.Entities.Core.Items.Themes
{
    public interface IDungeonTheme
    {
        void ConfigureDungeon(DungeonBuilder builder);

        string GetIntroMessage();

        string GetThemeName();
        void AddThemeContent(World world);
    }
}
