using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameState : MonoBehaviour
    {
        public static readonly Vector2Int MapSize = new Vector2Int(14, 8);

        public static bool IsPaused { get; set; }

        public GameObject WavesParent;
        public GameObject EnemiesParent;

        public GameObject CreatePosition;
        public GameObject DestroyPosition;
        public GameObject StartPosition;
        public GameObject EndPosition;

        public Text GoldText;
        public Text LivesText;
        public Text LivesLostText;

        public List<Vector2> Path { get; private set; }
        public bool IsWaveActive => _currWave >= 0 && (_waves[_currWave % _waves.Length].IsActive || EnemiesParent.transform.childCount > 0);
        public int Gold { get; private set; } = 100;
        public int Lives { get; private set; } = 20;
        public bool IsGameOver => Lives == 0;

        private Wave[] _waves;
        private int _currWave = -1;
        private float _livesLostTimer;

        private void Start()
        {
            IsPaused = false;
            _waves = WavesParent.GetComponentsInChildren<Wave>();
            UpdateGold(0);
            UpdateLives(0);
            LivesLostText.enabled = false;
        }

        private void Update()
        {
            if (IsPaused)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("Pause", LoadSceneMode.Additive);
                IsPaused = true;
                return;
            }

            if (_livesLostTimer > 0f)
            {
                _livesLostTimer -= Time.deltaTime;
                LivesLostText.enabled = _livesLostTimer > 0f;
            }
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
                _currWave++;
                _waves[_currWave % _waves.Length].StartWave(_currWave);
            }
        }

        public bool HasPath(Vector2 exclude)
        {
            return PathingHelper.ShortestPath(StartPosition.transform.position, EndPosition.transform.position, exclude).Count > 0;
        }

        public void UpdateGold(int delta)
        {
            Gold += delta;
            GoldText.text = "G: " + Gold;
        }

        public void UpdateLives(int delta)
        {
            Lives += delta;
            LivesText.text = "L: " + Lives;
            if (delta != 0)
            {
                LivesLostText.text = Convert.ToString(delta);
                _livesLostTimer = 0.5f;
            }

            if (IsGameOver)
            {
                SceneManager.LoadScene("Game Over", LoadSceneMode.Additive);
            }
        }
    }
}
