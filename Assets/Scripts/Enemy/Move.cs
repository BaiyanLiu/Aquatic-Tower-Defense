using System.Linq;
using Assets.Scripts.Effect;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public sealed class Move : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private EnemyBase _base;

        private int _currWaypoint;

        [UsedImplicitly]
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponentInChildren<Animator>();
            _base = GetComponent<EnemyBase>();
        }

        [UsedImplicitly]
        private void FixedUpdate()
        {
            if (GameState.Instance.IsPaused || GameState.Instance.IsGameOver || _base.Health <= 0f)
            {
                return;
            }

            var slowAmount = _base.Effects.OfType<SlowEffect>().Select(effect => effect.Amount.Value).Prepend(0f).Max();
            var p = Vector2.MoveTowards(transform.position, GameState.Instance.Path[_currWaypoint], _base.Speed * (1 - slowAmount));
            _rigidbody.MovePosition(p);

            if ((Vector2) transform.position == GameState.Instance.Path[_currWaypoint])
            {
                if (++_currWaypoint == GameState.Instance.Path.Count)
                {
                    GameState.Instance.UpdateLives(-_base.Lives);
                    Destroy(gameObject);
                    return;
                }
            }

            var dir = GameState.Instance.Path[_currWaypoint] - (Vector2) transform.position;
            _animator.SetFloat("DirX", dir.x);
            _animator.SetFloat("DirY", dir.y);
        }
    }
}
