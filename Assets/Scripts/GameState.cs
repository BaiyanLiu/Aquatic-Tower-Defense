using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Screens;
using Assets.Scripts.Tower;
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
        public Transform EnemiesParent;

        public Transform CreatePosition;
        public Transform DestroyPosition;
        public Transform StartPosition;
        public Transform EndPosition;

        public Text GoldText;
        public Text LivesText;
        public Text LivesLostText;
        public Text StartButtonText;

        public TowerDetails TowerDetails;

        public List<Vector2> Path { get; private set; }
        public bool IsWaveActive => _currWave >= 0 && (_waves[_currWave % _waves.Length].IsActive || EnemiesParent.childCount > 0);
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

            if (!IsWaveActive)
            {
                StartButtonText.text = "Start Wave " + (_currWave + 2);
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
                Path = PathingHelper.ShortestPath(StartPosition.position, EndPosition.position, Vector2.negativeInfinity);
                if (_currWave >= 0)
                {
                    _waves[_currWave % _waves.Length].OnCreateEnemy -= HandleCreateEnemy;
                }
                var wave = _waves[++_currWave % _waves.Length];
                wave.OnCreateEnemy += HandleCreateEnemy;
                wave.StartWave(_currWave);
            }
        }

        private void HandleCreateEnemy(object sender, float e)
        {
            StartButtonText.text = $"Wave {_currWave + 1} - {Math.Round(e * 100)}%";
        }

        public bool HasPath(Vector2 exclude)
        {
            return PathingHelper.ShortestPath(StartPosition.position, EndPosition.position, exclude).Count > 0;
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

        public void RegisterTower(GameObject tower)
        {
            tower.GetComponentInChildren<Interaction>().OnClick += (sender, args) =>
            {
                TowerDetails.UpdateTower(tower);
            };
        }
    }
}
