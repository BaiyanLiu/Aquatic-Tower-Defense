using System;
using System.Collections.Generic;
using Assets.Scripts.Effect;
using Assets.Scripts.Scenes;
using Assets.Scripts.Tower;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public sealed class EnemyBase : MonoBehaviour
    {
        public event EventHandler<GameObject> OnDie;
        public event EventHandler<GameObject> OnDestroyed;

        public float MaxHealthBase;
        public float ArmorBase;

        public float MaxHealthGain;
        public float ArmorGain;

        public float Speed;
        public ArmorType ArmorType;
        public int Experience;
        public int Gold;
        public int Lives;
        public float ItemChance;
        public string Name;

        public float MaxHealth { get; private set; }
        public float Armor { get; private set; }
        public float Health { get; private set; }
        public HashSet<EffectBase> Effects { get; } = new HashSet<EffectBase>();

        public int Level
        {
            set
            {
                MaxHealth = (MaxHealthBase + MaxHealthGain * value) * PlayerPrefs.GetInt(Settings.Health) / 100f;
                Armor = ArmorBase + ArmorGain * value;
                Health = MaxHealth;
            }
        }

        private float DamageReduction => (100f - Armor) / 100f;

        private Animator _animator;
        private Transform _healthBar;
        private SpriteRenderer[] _statusIndicators;
        private GameState _gameState;

        [UsedImplicitly]
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _healthBar = transform.Find("Status").Find("Health").Find("Fill");
            _statusIndicators = transform.Find("Status").Find("Indicators").GetComponentsInChildren<SpriteRenderer>();
            _gameState = GameState.GetGameState(gameObject);
        }

        [UsedImplicitly]
        private void Update()
        {
            if (GameState.IsPaused || _gameState.IsGameOver)
            {
                return;
            }

            Effects.RemoveWhere(effect => (effect.Duration -= Time.deltaTime) <= 0f);

            var statusColors = new SortedDictionary<string, Color>();
            foreach (var effect in Effects)
            {
                if (effect.UpdateTimer(Time.deltaTime))
                {
                    if (effect is PoisonEffect)
                    {
                        effect.Tower.EnemyAttacked(Math.Min(effect.Amount, Health));
                        if (UpdateHealth(-effect.Amount))
                        {
                            effect.Tower.EnemyKilled(this);
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

        [UsedImplicitly]
        private void OnDestroy()
        {
            OnDestroyed?.Invoke(this, gameObject);
        }

        public bool OnAttacked(float damage, TowerBase tower, List<EffectBase> effects)
        {
            Effects.UnionWith(effects);
            var healthDelta = damage * DamageReduction * DamageTypeReduction(tower.DamageType);
            tower.EnemyAttacked(Math.Min(healthDelta, Health));
            return UpdateHealth(-healthDelta);
        }

        private bool UpdateHealth(float delta)
        {
            if (Health > 0f)
            {
                Health += delta;
                _healthBar.localScale = new Vector2(Math.Max(Health / MaxHealth, 0f), 1f);
                if (Health <= 0f)
                {
                    OnDie?.Invoke(this, gameObject);
                    _gameState.UpdateGold(Gold);
                    _animator.SetBool("Dead", true);
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
