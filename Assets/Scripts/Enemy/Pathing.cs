using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    internal static class PathingHelper
    {
        public static Stack<Vector2> ShortestPath(Vector2 from, Vector2 to, Vector2 exclude)
        {
            exclude = Vector2Int.RoundToInt(exclude);
            var nodes = new List<Vector2> {Vector2Int.RoundToInt(to)};

            var minX = (int) Math.Round(from.x);
            var maxX = (int) Math.Round(to.x);
            for (var x = minX; x <= maxX; x++)
            {
                for (var y = -GameState.MapSize.y; y <= GameState.MapSize.y; y++)
                {
                    var key = new Vector2Int(x, y);
                    if (key != exclude)
                    {
                        nodes.Add(key);
                    }
                }
            }

            var dist = new Dictionary<Vector2, int>();
            var score = new Dictionary<Vector2, float>();
            foreach (var node in nodes)
            {
                dist[node] = int.MaxValue;
                score[node] = float.MaxValue;
            }

            var pos = Vector2Int.RoundToInt(from);
            dist[pos] = 0;
            score[pos] = Vector2.Distance(from, to);

            var openNodes = new Dictionary<Vector2, float>(score);
            var cameFrom = new Dictionary<Vector2, Vector2>();

            while (openNodes.Count > 0)
            {
                var curr = openNodes.OrderBy(x => x.Value).First().Key;
                if (dist[curr] == int.MaxValue)
                {
                    break;
                }

                if (curr == to)
                {
                    var path = new Stack<Vector2>();
                    while (cameFrom.ContainsKey(curr))
                    {
                        path.Push(curr);
                        if (from == cameFrom[curr])
                        {
                            return path;
                        }
                        curr = cameFrom[curr];
                    }
                }

                openNodes.Remove(curr);
                var neighbors = (
                    from dir in new[] { Vector2Int.right, Vector2Int.up, Vector2Int.left, Vector2Int.down }
                    let key = Vector2Int.RoundToInt(curr + dir)
                    where IsValid(curr, dir) && dist.ContainsKey(key) && (dir == Vector2Int.right || curr.x >= minX && curr.x <= maxX)
                    select key).ToList();

                foreach (var neighbor in neighbors)
                {
                    var altDist = dist[curr] + Dist(curr, neighbor);
                    if (altDist < dist[neighbor])
                    {
                        dist[neighbor] = altDist;
                        openNodes[neighbor] = score[neighbor] = altDist + Vector2.Distance(neighbor, to);
                        cameFrom[neighbor] = curr;
                    }
                }
            }

            return new Stack<Vector2>();
        }

        private static int Dist(Vector2 from, Vector2 to)
        {
            return Math.Abs((int) (from.x - to.x)) + Math.Abs((int) (from.y - to.y));
        }

        private static bool IsValid(Vector2 pos, Vector2 dir)
        {
            var hit = Physics2D.CircleCast(pos, 0.45f, dir, 1f, (1 << 31) + (1 << 30));
            return hit.collider == null;
        }

        public static List<Vector2> CollapsePath(Vector2 from, Stack<Vector2> path)
        {
            var newPath = new List<Vector2>();

            var collapsedItem = path.Pop();
            var dir = collapsedItem - from;
            var collapseX = Math.Abs(dir.y) > 0.1f;
            var collapseY = Math.Abs(dir.x) > 0.1f;

            foreach (var item in path)
            {
                if (Math.Abs(item.x - collapsedItem.x) < 0.1f && collapseX)
                {
                    collapsedItem.y = item.y;
                }
                else if (Math.Abs(item.y - collapsedItem.y) < 0.1f && collapseY)
                {
                    collapsedItem.x = item.x;
                }
                else
                {
                    newPath.Add(collapsedItem);
                    collapsedItem = item;
                    collapseX = !collapseX;
                    collapseY = !collapseY;
                }
            }
            newPath.Add(collapsedItem);

            return newPath;
        }
    }
}
