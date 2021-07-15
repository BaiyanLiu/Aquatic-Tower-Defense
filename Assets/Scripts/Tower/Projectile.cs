using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Effect;
using Assets.Scripts.Effect.Innate;
using Assets.Scripts.Enemy;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    public sealed class Projectile : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;

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

        [UsedImplicitly]
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            GetComponent<SpriteRenderer>().color = _color;
        }

        [UsedImplicitly]
        private void FixedUpdate()
        {
            if (GameState.Instance.IsPaused || GameState.Instance.IsGameOver)
            {
                return;
            }

            if (_target == null)
            {
                Destroy(gameObject);
                return;
            }

            var p = Vector2.MoveTowards(transform.position, _target.transform.position, _tower.ProjectileSpeed.Value);
            _rigidbody.MovePosition(p);
        }

        [UsedImplicitly]
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject == _target)
            {
                _prevTargets.Add(_target);

                var enemies = new HashSet<EnemyBase> { _target.GetComponent<EnemyBase>()};

                if (_tower.AllEffects.LastOrDefault(effect => effect is SplashEffect) is SplashEffect splashEffect)
                {
                    var hits = Physics2D.OverlapCircleAll(transform.position, splashEffect.Amount.Value, 1 << 29);
                    foreach (var hit in hits)
                    {
                        enemies.Add(hit.gameObject.GetComponent<EnemyBase>());
                    }
                }

                var effects = _tower.AllEffects.Where(effect => !effect.IsInnate && effect.Source == _tower).Select(effect => (EffectBase) effect.Clone()).ToList();
                foreach (var enemy in enemies.Where(enemy => enemy.OnAttacked(_damage, _tower, effects)))
                {
                    _tower.EnemyKilled(enemy);
                }

                if (_tower.AllEffects.LastOrDefault(effect => effect is ChainEffect) is ChainEffect chainEffect)
                {
                    _damage *= chainEffect.Amount.Value;
                    if (_damage >= 1f)
                    {
                        var hit = Physics2D.OverlapCircleAll(transform.position, chainEffect.Range.Value, 1 << 29)
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
