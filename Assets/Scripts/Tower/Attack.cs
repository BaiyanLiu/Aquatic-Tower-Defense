using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class Attack : MonoBehaviour
    {
        public GameObject Projectile;

        private Base _base;

        private readonly List<GameObject> _enemies = new List<GameObject>();
        private float _attackTimer;

        private void Start()
        {
            _base = GetComponent<Base>();
        }

        private void FixedUpdate()
        {
            _attackTimer -= Time.deltaTime;
            if (_attackTimer < 0)
            {
                AttackEnemy();
                _attackTimer = _base.AttackSpeed;
            }
        }

        private void AttackEnemy()
        {
            var enemy = _enemies.FirstOrDefault();
            if (enemy != null)
            {
                var projectile = Instantiate(Projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
                projectile.Target = enemy;
                projectile.Damage = _base.Damage;
                projectile.Speed = _base.ProjectileSpeed;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name.StartsWith("Enemy"))
            {
                _enemies.Add(collision.gameObject);
                collision.gameObject.GetComponent<Enemy.Base>().OnDie += (sender, args) =>
                {
                    _enemies.Remove(args);
                };
            }
        }
        
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.name.StartsWith("Enemy"))
            {
                _enemies.Remove(collision.gameObject);
            }
        }
    }
}
