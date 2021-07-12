using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public sealed class Wave : MonoBehaviour
    {
        public event EventHandler<float> OnCreateEnemy;
        public event EventHandler OnWaveCleared;

        public GameObject[] Enemies;

        public bool IsActive { get; set; }

        private int _level;
        private float _createEnemyTimer;
        private int _currEnemy;
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
                enemy.GetComponent<EnemyBase>().Level = _level;
                GameState.Instance.RegisterEnemy(enemy);
                OnCreateEnemy?.Invoke(this, (float) ++_currEnemy / Enemies.Length);

                if (_currEnemy == Enemies.Length)
                {
                    IsActive = false;
                    return;
                }

                _createEnemyTimer = 1f;
            }
        }

        public void StartWave(int level)
        {
            IsActive = true;
            _level = level;
            _currEnemy = 0;
            _isCleared = false;
        }
    }
}
