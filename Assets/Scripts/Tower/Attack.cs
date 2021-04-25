using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class Attack : MonoBehaviour
    {
        public GameObject Projectile;

        private readonly List<GameObject> _enemies = new List<GameObject>();
        private float _attackTimer;


        private void FixedUpdate()
        {
            _attackTimer -= Time.deltaTime;
            if (_attackTimer < 0)
            {
                AttackEnemy();
                _attackTimer = GetComponent<Base>().AttackSpeed;
            }
        }

        private void AttackEnemy()
        {
            if (_enemies.Count > 0)
            {
                var projectile = Instantiate(Projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
                projectile.Target = _enemies[0];
                projectile.Speed = GetComponent<Base>().ProjectileSpeed;
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
