using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class Projectile : MonoBehaviour
    {
        public GameObject Target;

        private void FixedUpdate()
        {
            var p = Vector2.MoveTowards(transform.position, Target.transform.position, 0.2f);
            GetComponent<Rigidbody2D>().MovePosition(p);
        }
    }
}
