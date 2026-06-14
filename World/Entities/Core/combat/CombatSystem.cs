using ConOOD_RpgGame.World.Entities;
using ConOOD_RpgGame.World.Entities.Core.Items;
using ConOOD_RpgGame.World.Entities.Core.Logging;

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

            Item equippedItem = player.GetEquippedCombatItem();

            attackStrategy.Execute(equippedItem, player);

            int rawPlayerDamage = attackStrategy.Damage;
            int actualPlayerDamage = rawPlayerDamage - enemy.Armor;

            if (actualPlayerDamage < 0)
            {
                actualPlayerDamage = 0;
            }

            enemy.ReceiveDamage(rawPlayerDamage);
            enemy.OnAttacked();
            GameLogger.Instance.Log($"Player dealt {actualPlayerDamage} damage.");

            string result = $"{attackStrategy.Name} attack dealt {actualPlayerDamage} damage.";

            if (!enemy.IsAlive)
            {
                enemy.BroadcastDeath();
                enemy.UnsubscribeFromSpecies();

                GameLogger.Instance.Log($"{enemy.Name} defeated.");
                return result + $" {enemy.Name} defeated.";
            }

            int enemyDamage = enemy.Attack - attackStrategy.Defense;

            if (enemyDamage < 0)
            {
                enemyDamage = 0;
            }

            player.ReceiveDamage(enemyDamage);

            GameLogger.Instance.Log($"{enemy.Name} dealt {enemyDamage} damage.");

            result += $" {enemy.Name} dealt {enemyDamage} damage.";

            if (!player.IsAlive)
            {
                result += " You died. Game over.";
            }

            return result;
        }
    }
}