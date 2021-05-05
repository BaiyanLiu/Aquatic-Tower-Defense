using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class Projectile : MonoBehaviour
    {
        public GameObject Target { private get; set; }
        public float Damage { private get; set; }
        public float Speed { private get; set; }
        public float Splash { private get; set; }

        private Rigidbody2D _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (Target == null)
            {
                Destroy(gameObject);
                return;
            }

            var p = Vector2.MoveTowards(transform.position, Target.transform.position, Speed);
            _rigidbody.MovePosition(p);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject == Target)
            {
                var enemies = new HashSet<Enemy.Base> {Target.GetComponent<Enemy.Base>()};
                if (Splash > 0f)
                {
                    var hits = Physics2D.OverlapCircleAll(transform.position, Splash, 1 << 29);
                    foreach (var hit in hits)
                    {
                        enemies.Add(hit.gameObject.GetComponent<Enemy.Base>());
                    }
                }

                foreach (var enemy in enemies)
                {
                    enemy.UpdateHealth(-Damage);
                }
                Destroy(gameObject);
            }
        }
    }
}
