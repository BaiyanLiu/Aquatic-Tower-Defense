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
        public int Gold;
        public int Lives;
        
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
        private SpriteRenderer[] _statusIndicators;
        private GameState _gameState;

        private float _maxHealth;
        private float _armor;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _healthBar = transform.Find("Status").Find("Health").Find("Fill");
            _statusIndicators = transform.Find("Status").Find("Indicators").GetComponentsInChildren<SpriteRenderer>();
            _gameState = GameState.GetGameState(gameObject);
        }

        private void Update()
        {
            if (_gameState.IsGameOver)
            {
                return;
            }

            Effects.RemoveWhere(effect => (effect.Duration -= Time.deltaTime) <= 0f);

            var statusColors = new SortedDictionary<string, Color>();
            foreach (var effect in Effects)
            {
                if (effect.UpdateTimer(Time.deltaTime))
                {
                    if (effect is PoisonEffect poisonEffect)
                    {
                        if (UpdateHealth(-poisonEffect.Damage))
                        {
                            effect.Tower.UpdateExperience(Experience);
                        }
                    }
                }
                statusColors[effect.Name] = effect.StatusColor;
            }

            var index = 0;
            foreach (var statusColor in statusColors.Values)
            {
                _statusIndicators[index].color = statusColor;
                _statusIndicators[index].enabled = true;
                index++;
            }
            for (var i = index; i < _statusIndicators.Length; i++)
            {
                _statusIndicators[i].enabled = false;
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
                    _gameState.UpdateGold(Gold);
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
