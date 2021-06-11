using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Enemy;
using Assets.Scripts.Item;
using Assets.Scripts.Scenes;
using Assets.Scripts.Screens;
using Assets.Scripts.Tower;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

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

        public GameObject PathTile;
        public TowerDetails TowerDetails;
        public EnemyDetails EnemyDetails;
        public Inventory Inventory;

        public ItemBase[] ItemPool;

        public List<Vector2> Path { get; private set; }
        public bool IsWaveActive => _currWave >= 0 && (_waves[_currWave % _waves.Length].IsActive || EnemiesParent.childCount > 0);
        public int Gold { get; private set; } = 100;
        public int Lives { get; private set; } = 20;
        public bool IsGameOver => Lives == 0;
        public bool IsBuilding { get; set; }
        public List<ItemBase> Items { get; } = new List<ItemBase>();

        private readonly Random _random = new Random();

        private Wave[] _waves;
        private int _currWave = -1;
        private float _livesLostTimer;
        private readonly List<GameObject> _pathTiles = new List<GameObject>();

        [UsedImplicitly]
        private void Start()
        {
            Settings.Init();
            IsPaused = false;
            _waves = WavesParent.GetComponentsInChildren<Wave>();

            UpdateGold(0);
            UpdateLives(0);
            LivesLostText.enabled = false;
            Inventory.GetComponent<RectTransform>().anchoredPosition = new Vector2(PlayerPrefs.GetFloat(Settings.InventoryX), PlayerPrefs.GetFloat(Settings.InventoryY));
        }

        [UsedImplicitly]
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

                if (_pathTiles.Any())
                {
                    _pathTiles.ForEach(Destroy);
                    _pathTiles.Clear();
                }
            }
        }

        public static GameState GetGameState(GameObject gameObject)
        {
            return gameObject.scene.GetRootGameObjects().First(o => o.name == "Main Camera").GetComponent<GameState>();
        }

        [UsedImplicitly]
        public void StartWave()
        {
            if (!IsWaveActive)
            {
                var path = PathingHelper.ShortestPath(CreatePosition.position, DestroyPosition.position, Vector2.negativeInfinity);
                foreach (var step in path)
                {
                    _pathTiles.Add(Instantiate(PathTile, step, Quaternion.identity));
                }
                Path = PathingHelper.CollapsePath(CreatePosition.position, path);

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
                if (!IsBuilding)
                {
                    EnemyDetails.UpdateTarget(null);
                    TowerDetails.UpdateTarget(tower);
                }
            };
        }

        public void RegisterEnemy(GameObject enemy)
        {
            enemy.GetComponent<Interaction>().OnClick += (sender, args) =>
            {
                TowerDetails.UpdateTarget(null);
                EnemyDetails.UpdateTarget(enemy);
            };
        }

        public void EnemyKilled(TowerBase tower, EnemyBase enemy)
        {
            if (_random.NextDouble() <= enemy.ItemChance)
            {
                var item = (ItemBase) ItemPool[_random.Next(ItemPool.Length)].Clone();
                item.Level = _currWave + 1;
                Inventory.AddItem(item);
                tower.AddItem(item);
            }
        }
    }
}
