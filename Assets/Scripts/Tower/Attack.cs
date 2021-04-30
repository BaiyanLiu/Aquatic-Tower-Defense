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
        private GameObject _target;
        private float _attackTimer;

        private void Start()
        {
            _base = GetComponent<Base>();
        }

        private void FixedUpdate()
        {
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
                _attackTimer = _base.AttackSpeed;
            }
        }

        private void AttackEnemy()
        {
            var projectile = Instantiate(Projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
            projectile.Target = _target;
            projectile.Damage = _base.Damage;
            projectile.Speed = _base.ProjectileSpeed;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name.StartsWith("Enemy"))
            {
                _enemies.Add(collision.gameObject);
                _target = _enemies.First();
                collision.gameObject.GetComponent<Enemy.Base>().OnDie += (sender, args) =>
                {
                    _enemies.Remove(args);
                    _target = _enemies.FirstOrDefault();
                };
            }
        }
        
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.name.StartsWith("Enemy"))
            {
                _enemies.Remove(collision.gameObject);
                _target = _enemies.FirstOrDefault();
            }
        }
    }
}
