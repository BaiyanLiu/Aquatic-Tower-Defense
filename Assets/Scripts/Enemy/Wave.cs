using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public sealed class Wave : MonoBehaviour
    {
        public event EventHandler<float> OnCreateEnemy;
        public event EventHandler OnWaveCleared;

        public GameObject[] Enemies;

        public bool IsActive { get; private set; }

        private int _level;
        private float _createEnemyTimer;
        private int _currEnemy;
        private readonly List<GameObject> _activeEnemies = new List<GameObject>();
        private bool _isForceStopped;

        [UsedImplicitly]
        private void Update()
        {
            if (GameState.Instance.IsPaused || GameState.Instance.IsGameOver || !IsActive)
            {
                return;
            }

            _createEnemyTimer -= Time.deltaTime;
            if (_createEnemyTimer <= 0f)
            {
                var enemy = Instantiate(Enemies[_currEnemy], GameState.Instance.CreatePosition.position, Quaternion.identity, GameState.Instance.EnemiesParent);
                _activeEnemies.Add(enemy);
                GameState.Instance.RegisterEnemy(enemy);

                var enemyBase = enemy.GetComponent<EnemyBase>();
                enemyBase.Level = _level;
                enemyBase.OnDestroyed += HandleEnemyDestroyed;
                _createEnemyTimer = 1f;

                OnCreateEnemy?.Invoke(this, (float) ++_currEnemy / Enemies.Length);
                if (_currEnemy == Enemies.Length)
                {
                    StopWave();
                }
            }
        }

        private void HandleEnemyDestroyed(object sender, GameObject enemy)
        {
            if (_isForceStopped)
            {
                return;
            }

            _activeEnemies.Remove(enemy);
            if (!IsActive && !_activeEnemies.Any())
            {
                OnWaveCleared?.Invoke(this, EventArgs.Empty);
            }
        }

        public void StartWave(int level)
        {
            IsActive = true;
            _level = level;
            _activeEnemies.Clear();
        }

        public void StopWave(bool force = false)
        {
            IsActive = false;
            _currEnemy = 0;

            _isForceStopped = force;
            if (_isForceStopped)
            {
                _activeEnemies.ForEach(Destroy);
                _activeEnemies.Clear();
                _isForceStopped = false;
            }
        }
    }
}
