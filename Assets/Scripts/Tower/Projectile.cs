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
            GetComponent<SpriteRenderer>().color = _color;
        }

        private void FixedUpdate()
        {
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
                if (_tower.Splash > 0f)
                {
                    var hits = Physics2D.OverlapCircleAll(transform.position, _tower.Splash, 1 << 29);
                    foreach (var hit in hits)
                    {
                        enemies.Add(hit.gameObject.GetComponent<EnemyBase>());
                    }
                }

                var effects = _tower.Effects.Select(effect => (EffectBase) effect.Clone()).ToList();
                foreach (var enemy in enemies.Where(enemy => enemy.OnAttacked(-_damage, _tower.DamageType, effects)))
                {
                    _tower.UpdateExperience(enemy.Experience);
                }

                _damage *= _tower.ChainDamage;
                if (_damage >= 1f)
                {
                    var hit = Physics2D.OverlapCircleAll(transform.position, _tower.ChainRange, 1 << 29)
                        .FirstOrDefault(t => !_prevTargets.Contains(t.gameObject));
                    if (hit != null)
                    { 
                        Create(gameObject, _color, transform.position, _tower, _damage, hit.gameObject, _prevTargets);
                    }
                }

                Destroy(gameObject);
            }
        }
    }
}
