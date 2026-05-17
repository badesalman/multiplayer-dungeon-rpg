using System.Collections.Generic;

namespace ConOOD_RpgGame.World.Entities.Core.Navigation
{
    public class DungeonNavigator
    {
        public int GetDistance(
           World world,
            Position start,
            Position target,
            int maxDistance)
        {
            Queue<(Position position, int distance)> queue =
                new Queue<(Position, int)>();
            bool[,] visited = new bool[World.Rows, World.Cols];

            queue.Enqueue((start, 0));
            visited[start.Row, start.Col] = true;

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (current.position.Row == target.Row &&
                    current.position.Col == target.Col)
                {
                    return current.distance;
                }

                if (current.distance >= maxDistance)
                {
                    continue;
                }

                List<Position> neighbors = new List<Position>
                {
                    new Position(current.position.Row - 1, current.position.Col),
                    new Position(current.position.Row + 1, current.position.Col),
                    new Position(current.position.Row, current.position.Col - 1),
                    new Position(current.position.Row, current.position.Col + 1)
                };

                foreach (Position neighbor in neighbors)
                {
                    if (!world.IsInside(neighbor))
                    {
                        continue;
                    }

                    if (visited[neighbor.Row, neighbor.Col])
                    {
                        continue;
                    }

                    Tile tile = world.GetTile(neighbor.Row, neighbor.Col);

                    if (!world.CanEnter(neighbor))
                    {
                        continue;
                    }

                    visited[neighbor.Row, neighbor.Col] = true;

                    queue.Enqueue((
                        neighbor,
                        current.distance + 1
                    ));
                }
            }

            return -1;
        }
    }
}