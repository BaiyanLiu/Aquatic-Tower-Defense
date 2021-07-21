using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Effect.Innate;
using Assets.Scripts.Enemy;
using Assets.Scripts.Item;
using Assets.Scripts.Persistence;
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
    public sealed class GameState : MonoBehaviour, IHasItems
    {
        public static readonly Vector2Int MapSize = new Vector2Int(14, 8);

        public static GameState Instance { get; private set; }

        public GameObject WavesParent;
        public Transform EnemiesParent;

        public Transform CreatePosition;
        public Transform DestroyPosition;
        public Transform StartPosition;
        public Transform EndPosition;

        public Text GoldText;
        public Text CostText;
        public Text LivesText;
        public Text LivesLostText;
        public Text StartButtonText;

        public GameObject PathTile;
        public TowerDetails TowerDetails;
        public EnemyDetails EnemyDetails;
        public Inventory Inventory;
        public WavePreview WavePreview;

        public ItemBase[] ItemPool;

        public bool IsPaused { get; set; }
        public bool IsBuilding { get; set; }

        public bool IsWaveActive => _currWave >= 0 && (_waves[_currWave % _waves.Length].IsActive || EnemiesParent.childCount > 0);
        public List<Vector2> Path { get; private set; }

        public int Gold { get; private set; } = 100;
        public int Lives { get; private set; } = 20;
        public bool IsGameOver => Lives == 0;

        public List<ItemBase> Items { get; } = new List<ItemBase>();
        public bool IsInventoryFull => Items.Count == 36;

        private readonly Random _random = new Random();

        public readonly Dictionary<string, GameObject> TowersByName = new Dictionary<string, GameObject>();
        public readonly Dictionary<string, ItemBase> ItemsByName = new Dictionary<string, ItemBase>();

        private Wave[] _waves;
        private int _currWave = -1;
        private float _livesLostTimer;
        private readonly List<GameObject> _pathTiles = new List<GameObject>();
        private int _cost;

        private bool _isLoading;
        private Snapshot _snapshot;
        private readonly Dictionary<GameObject, TowerBase> _activeTowers = new Dictionary<GameObject, TowerBase>();

        [UsedImplicitly]
        private void Start()
        {
            Instance = this;

            IsPaused = false;
            _waves = WavesParent.GetComponentsInChildren<Wave>();

            foreach (var item in ItemPool)
            {
                ItemsByName.Add(item.Name, item);
            }

            UpdateGold(0);
            UpdateCost(null);
            UpdateLives(0);
            LivesLostText.enabled = false;
            Inventory.GetComponent<RectTransform>().anchoredPosition = new Vector2(PlayerPrefs.GetFloat(Settings.InventoryX), PlayerPrefs.GetFloat(Settings.InventoryY));
            ResetWaveStatus();

            _snapshot = new Snapshot();
            UpdateSnapshot();
        }

        [UsedImplicitly]
        private void Update()
        {
            if (IsPaused)
            {
                return;
            }

            if (PlayerPrefs.GetInt(Settings.Load) == 1)
            {
                Load();
                PlayerPrefs.SetInt(Settings.Load, 0);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("Pause", LoadSceneMode.Additive);
                IsPaused = true;
                return;
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                Inventory.gameObject.SetActive(!Inventory.gameObject.activeSelf);
                Inventory.ItemDetails.UpdateTarget(null);
            }

            if (_livesLostTimer > 0f)
            {
                _livesLostTimer -= Time.deltaTime;
                LivesLostText.enabled = _livesLostTimer > 0f;
            }

            CostText.color = _cost > Gold ? Colors.Instance.Red : Colors.Instance.Yellow;
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
                    _waves[_currWave % _waves.Length].OnWaveCleared -= HandleWaveCleared;
                }
                var wave = _waves[++_currWave % _waves.Length];
                wave.OnCreateEnemy += HandleCreateEnemy;
                wave.OnWaveCleared += HandleWaveCleared;
                wave.StartWave(_currWave);
                WavePreview.UpdateWave(null, -1);
            }
        }

        private void HandleCreateEnemy(object sender, float e)
        {
            StartButtonText.text = $"Wave {_currWave + 1} - {Math.Round(e * 100)}%";
        }

        private void HandleWaveCleared(object sender, EventArgs e)
        {
            foreach (var tower in _activeTowers.Values)
            {
                tower.UpdateExperience(20);
            }
            UpdateGold(50);
            ResetWaveStatus();
            UpdateSnapshot();
        }

        private void ResetWaveStatus()
        {
            StartButtonText.text = "Start Wave " + (_currWave + 2);
            if (_pathTiles.Any())
            {
                _pathTiles.ForEach(Destroy);
                _pathTiles.Clear();
            }
            WavePreview.UpdateWave(_waves[(_currWave + 1) % _waves.Length], _currWave + 1);
        }

        public bool HasPath(Vector2 exclude)
        {
            return PathingHelper.ShortestPath(StartPosition.position, EndPosition.position, exclude).Count > 0;
        }

        public void UpdateGold(int delta, TowerBase tower = null)
        {
            var goldAmount = 100f;
            if (tower != null)
            {
                goldAmount = tower.AllEffects.OfType<GoldEffect>().Select(effect => effect.Amount.Value).Prepend(100f).Max();
            }
            Gold += (int) (delta * goldAmount / 100f);
            GoldText.text = "G: " + Gold;
        }

        public void UpdateCost(int? cost)
        {
            if (cost != null)
            {
                _cost = cost.Value;
                CostText.text = "-" + _cost;
            }
            else
            {
                CostText.text = "";
            }
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
            var towerBase = tower.GetComponentInChildren<TowerBase>();
            _activeTowers.Add(tower, towerBase);
            towerBase.OnDestroyed += HandleTowerDestroyed;

            var interaction = tower.GetComponentInChildren<Interaction>();
            interaction.OnClick += HandleTowerClick;
            interaction.OnEnter += HandleTowerMouseEnter;
            interaction.OnExit += HandleTowerMouseExit;
        }

        private void HandleTowerDestroyed(object sender, GameObject tower)
        {
            _activeTowers.Remove(tower);
        }

        private void HandleTowerClick(object sender, GameObject tower)
        {
            ActivateTowerDetails(tower, false);
        }

        private void HandleTowerMouseEnter(object sender, GameObject tower)
        {
            ActivateTowerDetails(tower, true);
        }

        private void ActivateTowerDetails(GameObject tower, bool isTemp)
        {
            if (!IsBuilding)
            {
                TowerDetails.UpdateTarget(tower.transform.parent.gameObject, isTemp);
            }
        }

        private void HandleTowerMouseExit(object sender, EventArgs e)
        {
            TowerDetails.UpdateTarget(null);
        }

        public void RegisterEnemy(GameObject enemy)
        {
            var interaction = enemy.GetComponent<Interaction>();
            interaction.OnClick += HandleEnemyClick;
            interaction.OnEnter += HandleEnemyMouseEnter;
            interaction.OnExit += HandleEnemyMouseExit;
        }

        private void HandleEnemyClick(object sender, GameObject enemy)
        {
            EnemyDetails.UpdateTarget(enemy, false);
        }

        private void HandleEnemyMouseEnter(object sender, GameObject enemy)
        {
            EnemyDetails.UpdateTarget(enemy);
        }

        private void HandleEnemyMouseExit(object sender, EventArgs e)
        {
            EnemyDetails.UpdateTarget(null);
        }

        public void EnemyKilled(TowerBase tower, EnemyBase enemy)
        {
            UpdateGold(enemy.Gold, tower);

            var towerItemChance = tower.AllEffects.OfType<ItemChanceEffect>().Select(effect => effect.Amount.Value).Prepend(100f).Max();
            while (!IsInventoryFull && enemy.ItemChance > 0f)
            {
                if (_random.NextDouble() <= enemy.ItemChance * towerItemChance / 100f)
                {
                    var item = (ItemBase) ItemPool[_random.Next(ItemPool.Length)].Clone();
                    item.UpdateLevel(_currWave + 1 + enemy.ItemLevelBonus);
                    AddItem(item);
                }
                enemy.ItemChance -= 1f;
            }
        }

        public void AddItem(ItemBase item)
        {
            Items.Add(item);
            Inventory.AddItem(item);
        }

        public void RemoveItem(int index)
        {
            Items.RemoveAt(index);
            Inventory.RemoveItem(index);
        }

        public void RegisterTowerForName(string towerName, GameObject prefab)
        {
            TowersByName.Add(towerName, prefab);
        }

        public void Save()
        {
            IsPaused = true;
            UpdateSnapshot();
            SaveUtils.Save(_snapshot);
            IsPaused = false;
        }

        public void Load()
        {
            IsPaused = true;

            var snapshot = SaveUtils.Load();
            if (snapshot != null)
            {
                _isLoading = true;

                Gold = snapshot.Gold;
                Lives = snapshot.Lives;
                UpdateGold(0);
                UpdateLives(0);

                if (IsWaveActive)
                {
                    var wave = _waves[_currWave % _waves.Length];
                    wave.OnCreateEnemy -= HandleCreateEnemy;
                    wave.OnWaveCleared -= HandleWaveCleared;
                    wave.IsActive = false;

                    foreach (Transform enemy in EnemiesParent)
                    {
                        Destroy(enemy.gameObject);
                    }
                }
                _currWave = snapshot.Wave;
                ResetWaveStatus();

                foreach (var tower in _activeTowers.Keys)
                {
                    Destroy(tower);
                }
                foreach (var tower in snapshot.Towers)
                {
                    RegisterTower(TowerBase.FromSnapshot(tower));
                }

                Items.Clear();
                Inventory.ResetItems();
                foreach (var item in snapshot.Items)
                {
                    AddItem(ItemBase.FromSnapshot(item));
                }

                TowerDetails.UpdateTarget(null, false);
                EnemyDetails.UpdateTarget(null, false);

                _snapshot = snapshot;
                _isLoading = false;
            }

            IsPaused = false;
        }

        private void UpdateSnapshot()
        {
            if (_isLoading || IsWaveActive)
            {
                return;
            }

            _snapshot.Gold = Gold;
            _snapshot.Lives = Lives;
            _snapshot.Wave = _currWave;
            _snapshot.Towers = _activeTowers.Keys.Select(tower => _activeTowers[tower].ToSnapshot()).ToArray();
            _snapshot.Items = Items.Select(item => item.ToSnapshot()).ToArray();
        }
    }
}
