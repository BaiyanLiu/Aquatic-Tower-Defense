using System.Collections.Generic;
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

        private List<Vector2> _waypoints;
        private int _currWaypoint;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _base = GetComponent<EnemyBase>();
            _gameState = GameState.GetGameState(gameObject);

            _waypoints = new List<Vector2> {_gameState.StartPosition.transform.position};
            _waypoints.AddRange(_gameState.Path);
            _waypoints.Add(_gameState.DestroyPosition.transform.position);
        }

        private void FixedUpdate()
        {
            if (_gameState.IsGameOver || _base.Health <= 0f)
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

            var p = Vector2.MoveTowards(transform.position, _waypoints[_currWaypoint], _base.Speed * (1 - slowAmount));
            _rigidbody.MovePosition(p);

            if ((Vector2) transform.position == _waypoints[_currWaypoint])
            {
                if (++_currWaypoint == _waypoints.Count)
                {
                    _gameState.UpdateLives(-_base.Lives);
                    Destroy(gameObject);
                    return;
                }
            }

            var dir = _waypoints[_currWaypoint] - (Vector2) transform.position;
            _animator.SetFloat("DirX", dir.x);
            _animator.SetFloat("DirY", dir.y);
        }
    }
}
