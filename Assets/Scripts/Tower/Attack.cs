using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Enemy;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    public sealed class Attack : MonoBehaviour
    {
        public GameObject Projectile;
        public Color ProjectileColor = Color.white;

        private TowerBase _base;

        private readonly List<GameObject> _enemies = new List<GameObject>();
        private GameObject _target;
        private float _attackTimer;

        [UsedImplicitly]
        private void Start()
        {
            _base = GetComponent<TowerBase>();
        }

        [UsedImplicitly]
        private void FixedUpdate()
        {
            if (GameState.Instance.IsPaused || GameState.Instance.IsGameOver)
            {
                return;
            }

            if (_target != null)
            {
                var dir = _target.transform.position - transform.position;
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

            _attackTimer -= Time.deltaTime;
            if (_attackTimer < 0 && _target != null)
            {
                AttackEnemy();
                _attackTimer = _base.AttackSpeed.Value;
            }
        }

        private void AttackEnemy()
        {
            Tower.Projectile.Create(Projectile, ProjectileColor, transform.position, _base, _base.Damage.Value, _target);
        }

        [UsedImplicitly]
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name.StartsWith("Enemy"))
            {
                _enemies.Add(collision.gameObject);
                _target = _enemies.First();

                var enemy = collision.gameObject.GetComponent<EnemyBase>();
                enemy.OnDie += HandleRemoveEnemy;
                enemy.OnDestroyed += HandleRemoveEnemy;
            }
        }

        private void HandleRemoveEnemy(object sender, GameObject enemy)
        {
            RemoveEnemy(enemy);
        }

        [UsedImplicitly]
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.name.StartsWith("Enemy"))
            {
                RemoveEnemy(collision.gameObject);
            }
        }

        private void RemoveEnemy(GameObject enemy)
        {
            _enemies.Remove(enemy);
            _target = _enemies.FirstOrDefault();
        }
    }
}
