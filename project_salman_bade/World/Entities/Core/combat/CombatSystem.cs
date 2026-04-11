using ConOOD_RpgGame.World.Entities;

namespace ConOOD_RpgGame.World.Entities.Core.combat
{
    public class CombatSystem
    {
        public string ExecuteTurn(Player player, Enemy enemy, AttackStrategy attackStrategy)
        {
            if (player == null || enemy == null || attackStrategy == null)
            {
                return "Combat could not start.";
            }

            if (!enemy.IsAlive)
            {
                return "Enemy is already defeated.";
            }

            var weapon = player.GetEquippedWeapon();

            int rawPlayerDamage = attackStrategy.CalculateDamage(player, weapon);
            int actualPlayerDamage = rawPlayerDamage - enemy.Armor;

            if (actualPlayerDamage < 0)
            {
                actualPlayerDamage = 0;
            }

            enemy.ReceiveDamage(rawPlayerDamage);

            string result = $"{attackStrategy.Name} attack dealt {actualPlayerDamage} damage.";

            if (!enemy.IsAlive)
            {
                return result + " Enemy defeated.";
            }

            int playerDefense = attackStrategy.CalculateDefense(player, weapon);
            int enemyDamage = enemy.Attack - playerDefense;

            if (enemyDamage < 0)
            {
                enemyDamage = 0;
            }

            player.ReceiveDamage(enemyDamage);

            result += $" Enemy dealt {enemyDamage} damage back.";

            if (player.IsDead())
            {
                result += " You lost the game.";
            }

            return result;
        }
    }
}