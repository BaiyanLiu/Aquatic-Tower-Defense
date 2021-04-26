using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class Move : MonoBehaviour
    { 
        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private Base _base;

        private Vector2 _dest;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _base = GetComponent<Base>();

            _dest = new Vector2(-transform.position.x, transform.position.y);
        }

        private void FixedUpdate()
        {
            if (_base.Health <= 0f)
            {
                return;
            }

            var p = Vector2.MoveTowards(transform.position, _dest, _base.Speed);
            _rigidbody.MovePosition(p);

            var dir = _dest - (Vector2) transform.position;
            _animator.SetFloat("DirX", dir.x);
            _animator.SetFloat("DirY", dir.y);
        }
    }
}
