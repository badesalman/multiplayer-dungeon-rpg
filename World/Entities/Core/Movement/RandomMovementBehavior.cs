using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConOOD_RpgGame.World.Entities.Core.Movement
{
    public class RandomMovementBehavior : IMovementBehavior
    {
        private static readonly Random random = new Random();

        public void Move(Enemy enemy, World world)
        {
            List<(int row, int col)> possibleMoves = new List<(int, int)>
            {
                (enemy.Position.Row - 1, enemy.Position.Col),
                (enemy.Position.Row + 1, enemy.Position.Col),
                (enemy.Position.Row, enemy.Position.Col - 1),
                (enemy.Position.Row, enemy.Position.Col + 1)
            };

            List<(int row, int col)> validMoves = new List<(int, int)>();

            foreach (var move in possibleMoves)
            {
                
                    Position position = new Position(move.row, move.col);

                    if (world.CanEnter(position))
                    {
                        validMoves.Add(move);
                    }
                
            }

            if (validMoves.Count == 0)
            {
                return;
            }

            var selectedMove = validMoves[random.Next(validMoves.Count)];
            if (!world.IsInside(new Position(selectedMove.row, selectedMove.col)))
            {
                return;
            }
            world.MoveEnemy(
     enemy,
     new Position(
         selectedMove.row,
         selectedMove.col));
        }
    }

}
