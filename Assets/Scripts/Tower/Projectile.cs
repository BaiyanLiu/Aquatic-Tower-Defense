using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class Projectile : MonoBehaviour
    {
        public GameObject Target { private get; set; }
        public float Damage { private get; set; }
        public float Speed { private get; set; }

        private void FixedUpdate()
        {
            var p = Vector2.MoveTowards(transform.position, Target.transform.position, Speed);
            GetComponent<Rigidbody2D>().MovePosition(p);
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
