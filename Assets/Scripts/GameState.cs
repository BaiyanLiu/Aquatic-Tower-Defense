using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameState : MonoBehaviour
    {
        public static readonly Vector2Int LevelSize = new Vector2Int(14, 8);

        public GameObject WavesParent;
        public GameObject EnemiesParent;

        public GameObject CreatePosition;
        public GameObject DestroyPosition;
        public GameObject StartPosition;
        public GameObject EndPosition;

        public List<Vector2> Path { get; private set; }
        public bool IsWaveActive => _currWave >= 0 && (_waves[_currWave].IsActive || EnemiesParent.transform.childCount > 0);

        private Wave[] _waves;
        private int _currWave = -1;

        private void Start()
        {
            _waves = WavesParent.GetComponentsInChildren<Wave>();
        }

        public static GameState GetGameState(GameObject gameObject)
        {
            return gameObject.scene.GetRootGameObjects().First(o => o.name == "Main Camera").GetComponent<GameState>();
        }

        public void StartWave()
        {
            if (!IsWaveActive)
            {
                Path = PathingHelper.ShortestPath(StartPosition.transform.position, EndPosition.transform.position, Vector2.negativeInfinity);
                _currWave = (_currWave + 1) % _waves.Length;
                _waves[_currWave].StartWave();
            }
        }

        public bool HasPath(Vector2 exclude)
        {
            return PathingHelper.ShortestPath(StartPosition.transform.position, EndPosition.transform.position, exclude).Count > 0;
        }
    }
}
