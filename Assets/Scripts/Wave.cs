using System;
using Assets.Scripts.Enemy;
using UnityEngine;

namespace Assets.Scripts
{
    public class Wave : MonoBehaviour
    {
        public event EventHandler<float> OnCreateEnemy;

        public GameObject[] Enemies;

        public bool IsActive { get; private set; }

        private GameState _gameState;

        private int _level;
        private float _createEnemyTimer;
        private int _currEnemy;

        private void Start()
        {
            _gameState = GameState.GetGameState(gameObject);
        }

        private void Update()
        {
            if (GameState.IsPaused || _gameState.IsGameOver || !IsActive)
            {
                return;
            }

            _createEnemyTimer -= Time.deltaTime;
            if (_createEnemyTimer <= 0f)
            {
                var enemy = Instantiate(Enemies[_currEnemy], _gameState.CreatePosition.transform.position, Quaternion.identity, _gameState.EnemiesParent.transform).GetComponent<EnemyBase>();
                enemy.Level = _level;
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
