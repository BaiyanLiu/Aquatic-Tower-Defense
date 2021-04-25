using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class Move : MonoBehaviour
    {
        private Vector2 _dest;

        private void Start()
        {
            _dest = new Vector2(-transform.position.x, transform.position.y);
        }

        private void FixedUpdate()
        {
            var p = Vector2.MoveTowards(transform.position, _dest, GetComponent<Base>().Speed);
            GetComponent<Rigidbody2D>().MovePosition(p);
        }
    }
}
