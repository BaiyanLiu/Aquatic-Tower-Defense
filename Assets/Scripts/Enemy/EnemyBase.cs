using System;
using System.Collections.Generic;
using Assets.Scripts.Effect;
using Assets.Scripts.Tower;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class EnemyBase : MonoBehaviour
    {
        public event EventHandler<GameObject> OnDie;

        public float MaxHealthBase;
        public float ArmorBase;

        public float MaxHealthGain;
        public float ArmorGain;

        public float Speed;
        public ArmorType ArmorType;
        public int Experience;
        
        public float Health { get; private set; }
        public HashSet<EffectBase> Effects { get; } = new HashSet<EffectBase>();

        public int Level
        {
            set
            {
                _maxHealth = MaxHealthBase + MaxHealthGain * value;
                _armor = ArmorBase + ArmorGain * value;
                Health = _maxHealth;
            }
        }

        private float DamageReduction => (100f - _armor) / 100f;

        private Animator _animator;
        private Transform _healthBar;

        private float _maxHealth;
        private float _armor;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _healthBar = transform.Find("Health Bar").Find("Fill");
        }

        private void Update()
        {
            Effects.RemoveWhere(effect => (effect.Duration -= Time.deltaTime) <= 0f);

            foreach (var effect in Effects)
            {
                if (effect is PoisonEffect poisonEffect && effect.UpdateTimer(Time.deltaTime))
                {
                    if (UpdateHealth(-poisonEffect.Damage))
                    {
                        effect.Tower.UpdateExperience(Experience);
                    }
                }
            }
        }

        public bool OnAttacked(float healthDelta, DamageType damageType, List<EffectBase> effects)
        {
            Effects.UnionWith(effects);
            return UpdateHealth(healthDelta * DamageReduction * DamageTypeReduction(damageType));
        }

        private bool UpdateHealth(float delta)
        {
            if (Health > 0f)
            {
                Health += delta;
                _healthBar.localScale = new Vector2(Math.Max(Health / _maxHealth, 0f), 1f);
                if (Health <= 0f)
                {
                    _animator.SetBool("Dead", true);
                    OnDie?.Invoke(this, gameObject);
                    return true;
                }
            }
            return false;
        }

        private float DamageTypeReduction(DamageType damageType)
        {
            return ArmorType switch
            {
                ArmorType.Light => damageType switch
                {
                    DamageType.Light => 1f,
                    DamageType.Normal => 1.1f,
                    DamageType.Heavy => 1.2f,
                    _ => 1f
                },
                ArmorType.Normal => damageType switch
                {
                    DamageType.Light => 0.8f,
                    DamageType.Normal => 1f,
                    DamageType.Heavy => 1.1f,
                    _ => 1f
                },
                ArmorType.Heavy => damageType switch
                {
                    DamageType.Light => 0.6f,
                    DamageType.Normal => 0.8f,
                    DamageType.Heavy => 1f,
                    _ => 1f
                },
                _ => 1f
            };
        }
    }
}
