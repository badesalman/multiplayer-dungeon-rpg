using ConOOD_RpgGame.World.Entities.Core.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConOOD_RpgGame.World.Entities.Core
{
    //defines all possible interactions between weapon types and attack types 
    //each attack implements this interface
    public interface IAttackVisitor
    {
        void VisitHeavyWeapon(HeavyWeapon weapon, Player player);
        void VisitLightWeapon(LightWeapon weapon, Player player);
        void VisitMagicalWeapon(MagicalWeapon weapon, Player player);
        void VisitItem(Item item, Player player);
    }
}
