using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class Move : MonoBehaviour
    {
        private readonly Vector2 _dest = new Vector2(10f, 0f);

        private void FixedUpdate()
        {
            var p = Vector2.MoveTowards(transform.position, _dest, 0.05f);
            GetComponent<Rigidbody2D>().MovePosition(p);
        }
    }
}
