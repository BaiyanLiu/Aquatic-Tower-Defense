using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Effect;
using Assets.Scripts.Enemy;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class Projectile : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;
        private GameState _gameState;

        private Color _color;
        private TowerBase _tower;
        private float _damage;
        private GameObject _target;
        private readonly ISet<GameObject> _prevTargets = new HashSet<GameObject>();

        public static void Create(GameObject gameObject, Color color, Vector2 position, TowerBase tower, float damage, GameObject target, ISet<GameObject> prevTargets = null)
        {
            var projectile = Instantiate(gameObject, position, Quaternion.identity).GetComponent<Projectile>();
            projectile._color = color;
            projectile._tower = tower;
            projectile._damage = damage;
            if (prevTargets != null)
            {
                projectile._prevTargets.UnionWith(prevTargets);
            }
            projectile._target = target;
        }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _gameState = GameState.GetGameState(gameObject);
            GetComponent<SpriteRenderer>().color = _color;
        }

        private void FixedUpdate()
        {
            if (GameState.IsPaused || _gameState.IsGameOver)
            {
                return;
            }

            if (_target == null)
            {
                Destroy(gameObject);
                return;
            }

            var p = Vector2.MoveTowards(transform.position, _target.transform.position, _tower.ProjectileSpeed);
            _rigidbody.MovePosition(p);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject == _target)
            {
                _prevTargets.Add(_target);

                var enemies = new HashSet<EnemyBase> { _target.GetComponent<EnemyBase>()};

                if (_tower.Effects.LastOrDefault(effect => effect is SplashEffect) is SplashEffect splashEffect)
                {
                    var hits = Physics2D.OverlapCircleAll(transform.position, splashEffect.Range, 1 << 29);
                    foreach (var hit in hits)
                    {
                        enemies.Add(hit.gameObject.GetComponent<EnemyBase>());
                    }
                }

                var effects = _tower.Effects.Where(effect => !effect.IsInnate).Select(effect => (EffectBase) effect.Clone()).ToList();
                foreach (var enemy in enemies.Where(enemy => enemy.OnAttacked(_damage, _tower, effects)))
                {
                    _tower.EnemyKilled(enemy);
                }

                if (_tower.Effects.LastOrDefault(effect => effect is ChainEffect) is ChainEffect chainEffect)
                {
                    _damage *= chainEffect.Damage;
                    if (_damage >= 1f)
                    {
                        var hit = Physics2D.OverlapCircleAll(transform.position, chainEffect.Range, 1 << 29)
                            .FirstOrDefault(t => !_prevTargets.Contains(t.gameObject));
                        if (hit != null)
                        {
                            Create(gameObject, _color, transform.position, _tower, _damage, hit.gameObject,
                                _prevTargets);
                        }
                    }
                }

                Destroy(gameObject);
            }
        }
    }
}
