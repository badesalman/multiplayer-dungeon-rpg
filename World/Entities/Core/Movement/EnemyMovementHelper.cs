using System.Collections.Generic;
using ConOOD_RpgGame.World.Entities;

namespace ConOOD_RpgGame.World.Entities.Core.Movement
{
    public static class EnemyMovementHelper
    {
        public static List<Position> GetValidMoves(Enemy enemy, World world)
        {
            List<Position> moves = new List<Position>
            {
                enemy.Position.Offset(-1, 0),
                enemy.Position.Offset(1, 0),
                enemy.Position.Offset(0, -1),
                enemy.Position.Offset(0, 1)
            };

            List<Position> valid = new List<Position>();

            foreach (Position move in moves)
            {
                if (world.IsInside(move) && world.CanEnter(move))
                {
                    valid.Add(move);
                }
            }

            return valid;
        }

        public static int Distance(Position a, Position b)
        {
            int row = a.Row - b.Row;
            int col = a.Col - b.Col;

            if (row < 0) row = -row;
            if (col < 0) col = -col;

            return row + col;
        }

        public static void MoveToward(Enemy enemy, World world, Position target)
        {
            List<Position> moves = GetValidMoves(enemy, world);

            if (moves.Count == 0)
            {
                return;
            }

            Position best = moves[0];
            int bestDistance = Distance(best, target);

            foreach (Position move in moves)
            {
                int distance = Distance(move, target);

                if (distance < bestDistance)
                {
                    best = move;
                    bestDistance = distance;
                }
            }

            world.MoveEnemy(enemy, best);
        }

        public static void MoveAwayFrom(Enemy enemy, World world, Position danger)
        {
            List<Position> moves = GetValidMoves(enemy, world);

            if (moves.Count == 0)
            {
                enemy.MoveDefault();
                return;
            }

            Position best = moves[0];
            int bestDistance = Distance(best, danger);

            foreach (Position move in moves)
            {
                int distance = Distance(move, danger);

                if (distance > bestDistance)
                {
                    best = move;
                    bestDistance = distance;
                }
            }

            if (bestDistance <= Distance(enemy.Position, danger))
            {
                enemy.MoveDefault();
                return;
            }

            world.MoveEnemy(enemy, best);
        }
    }
}
