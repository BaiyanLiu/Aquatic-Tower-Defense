using System.Collections.Generic;
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
            if (_enemies.Count > 0)
            {
                var projectile = Instantiate(Projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
                projectile.Target = _enemies[0];
                projectile.Damage = _base.Damage;
                projectile.Speed = _base.ProjectileSpeed;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name == "Enemy")
            {
                _enemies.Add(collision.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.name == "Enemy")
            {
                _enemies.Remove(collision.gameObject);
            }
        }
    }
}
