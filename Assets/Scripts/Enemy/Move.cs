using Assets.Scripts.Effect;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class Move : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private EnemyBase _base;
        private GameState _gameState;

        private int _currWaypoint;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _base = GetComponent<EnemyBase>();
            _gameState = GameState.GetGameState(gameObject);
        }

        private void FixedUpdate()
        {
            if (GameState.IsPaused || _gameState.IsGameOver || _base.Health <= 0f)
            {
                return;
            }

            var slowAmount = 0f;
            foreach (var effect in _base.Effects)
            {
                if (effect is SlowEffect slowEffect && slowEffect.Amount > slowAmount)
                {
                    slowAmount = slowEffect.Amount;
                }
            }

            var p = Vector2.MoveTowards(transform.position, _gameState.Path[_currWaypoint], _base.Speed * (1 - slowAmount));
            _rigidbody.MovePosition(p);

            if ((Vector2) transform.position == _gameState.Path[_currWaypoint])
            {
                if (++_currWaypoint == _gameState.Path.Count)
                {
                    _gameState.UpdateLives(-_base.Lives);
                    Destroy(gameObject);
                    return;
                }
            }

            var dir = _gameState.Path[_currWaypoint] - (Vector2) transform.position;
            _animator.SetFloat("DirX", dir.x);
            _animator.SetFloat("DirY", dir.y);
        }
    }
}
