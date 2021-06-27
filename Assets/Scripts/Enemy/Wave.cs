using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public sealed class Wave : MonoBehaviour
    {
        public event EventHandler<float> OnCreateEnemy;

        public GameObject[] Enemies;

        public bool IsActive { get; private set; }

        private int _level;
        private float _createEnemyTimer;
        private int _currEnemy;

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
                GameState.Instance.RegisterEnemy(enemy);
                enemy.GetComponent<EnemyBase>().Level = _level;
                _createEnemyTimer = 1f;

                OnCreateEnemy?.Invoke(this, (float) ++_currEnemy / Enemies.Length);
                if (_currEnemy == Enemies.Length)
                {
                    IsActive = false;
                    _currEnemy = 0;
                }
            }
        }

        public void StartWave(int level)
        {
            IsActive = true;
            _level = level;
        }
    }
}
