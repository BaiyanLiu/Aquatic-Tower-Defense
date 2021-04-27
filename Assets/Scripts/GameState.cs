using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameState : MonoBehaviour
    {
        private static readonly Vector2Int LevelSize = new Vector2Int(14, 8);

        public GameObject CreatePosition;
        public GameObject DestroyPosition;
        public GameObject StartPosition;
        public GameObject EndPosition;

        public GameObject Enemy;

        public Stack<Vector2> Path { get; private set; }

        private float _createEnemyTimer;

        private void Start()
        {
            Path = ShortestPath(StartPosition.transform.position, EndPosition.transform.position);
        }

        private void Update()
        {
            _createEnemyTimer -= Time.deltaTime;
            if (_createEnemyTimer <= 0f)
            {
                Instantiate(Enemy, CreatePosition.transform.position, Quaternion.identity);
                _createEnemyTimer = 1f;
            }
        }

        public static GameState GetGameState(GameObject gameObject)
        {
            return gameObject.scene.GetRootGameObjects().First(o => o.name == "Main Camera").GetComponent<GameState>();
        }

        private Stack<Vector2> ShortestPath(Vector2 from, Vector2 to)
        {
            var dist = new Dictionary<Vector2, int>();
            var score = new Dictionary<Vector2, float>();
            for (var x = -LevelSize.x; x <= LevelSize.x; x++)
            {
                for (var y = -LevelSize.y; y <= LevelSize.y; y++)
                {
                    var key = new Vector2Int(x, y);
                    dist[key] = int.MaxValue;
                    score[key] = float.MaxValue;
                }
            }
            var pos = Vector2Int.RoundToInt(from);
            dist[pos] = 0;
            score[pos] = Vector2.Distance(from, to);
            var openNodes = new Dictionary<Vector2, float>(score);
            var cameFrom = new Dictionary<Vector2, Vector2>();

            while (openNodes.Count > 0)
            {
                var curr = openNodes.OrderBy(x => x.Value).First().Key;
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
                    where IsValid(curr, dir) && dist.ContainsKey(key)
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

            return new Stack<Vector2>(new[] {Vector2.zero});
        }

        private int Dist(Vector2 from, Vector2 to)
        {
            return Math.Abs((int) (from.x - to.x)) + Math.Abs((int) (from.y - to.y));
        }

        private bool IsValid(Vector2 pos, Vector2 dir)
        {
            var hit = Physics2D.CircleCast(pos, 0.45f, dir, 1f, (1 << 31) + (1 << 30));
            return hit.collider == null;
        }
    }
}
