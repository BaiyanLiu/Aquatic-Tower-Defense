using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class Projectile : MonoBehaviour
    {
        public GameObject Target { private get; set; }
        public float Damage { private get; set; }
        public float Speed { private get; set; }

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
                Target.GetComponent<Enemy.Base>().UpdateHealth(-Damage);
                Destroy(gameObject);
            }
        }
    }
}
