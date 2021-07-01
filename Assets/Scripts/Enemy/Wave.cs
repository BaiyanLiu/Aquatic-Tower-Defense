using System;
using System.Collections.Generic;
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
        private bool _isCleared;

        [UsedImplicitly]
        private void Update()
        {
            if (GameState.Instance.IsPaused || GameState.Instance.IsGameOver)
            {
                return;
            }

            if (!IsActive)
            {
                if (!_isCleared && GameState.Instance.EnemiesParent.childCount == 0)
                {
                    OnWaveCleared?.Invoke(this, EventArgs.Empty);
                    _isCleared = true;
                }
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
            _activeEnemies.Remove(enemy);
        }

        public void StartWave(int level)
        {
            IsActive = true;
            _level = level;
            _isCleared = false;
        }

        public void StopWave(bool force = false)
        {
            IsActive = false;
            _currEnemy = 0;
            if (force)
            {
                _activeEnemies.ForEach(Destroy);
                _isCleared = true;
            }
        }
    }
}
