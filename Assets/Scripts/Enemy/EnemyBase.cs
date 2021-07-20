using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Effect;
using Assets.Scripts.Effect.Area;
using Assets.Scripts.Scenes;
using Assets.Scripts.Tower;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public sealed class EnemyBase : MonoBehaviour, IHasEffect
    {
        public event EventHandler<GameObject> OnDie;
        public event EventHandler<GameObject> OnDestroyed;

        public AttributeValue MaxHealth;
        public AttributeValue Armor;

        public float Speed;
        public ArmorType ArmorType;
        public int Experience;
        public int Gold;
        public int Lives;
        public string Name;

        public int Splits;
        private float _splitHealth;

        public float ItemChance;
        public int ItemLevelBonus;

        public float Health { get; private set; }
        public List<EffectBase> Effects { get; } = new List<EffectBase>();
        public HashSet<EffectBase> AllEffects { get; } = new HashSet<EffectBase>();

        private int _level;
        public int Level
        {
            set
            {
                _level = value;
                MaxHealth.Value = (MaxHealth.Base + MaxHealth.Gain * value) * PlayerPrefs.GetInt(Settings.Health) / 100f;
                Armor.Value = Armor.Base + Armor.Gain * value;
                Health = MaxHealth.Value;
                if (Splits > 0)
                {
                    _splitHealth = Health / Splits;
                }
            }
        }

        private float DamageReduction
        {
            get
            {
                var armorAmounts = AllEffects.OfType<AreaArmorEffect>().Select(effect => effect.Amount.Value).ToArray();
                var armorIncreaseAmount = armorAmounts.Where(amount => amount > 0f).Prepend(0f).Max();
                var armorDecreaseAmount = armorAmounts.Where(amount => amount < 0f).Prepend(0f).Min();
                return (100f - Armor.Value - armorIncreaseAmount - armorDecreaseAmount) / 100f;
            }
        }

        private Animator _animator;
        private Transform _healthBar;
        private SpriteRenderer[] _statusIndicators;

        [UsedImplicitly]
        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            _healthBar = transform.Find("Status").Find("Health").Find("Fill");
            _statusIndicators = transform.Find("Status").Find("Indicators").GetComponentsInChildren<SpriteRenderer>();
            UpdateHealth(0f);

            Effects.AddRange(GetComponents<EffectBase>());
            foreach (var effect in Effects)
            {
                effect.Source = this;
                effect.IncludeGain = false;
                effect.UpdateLevel(_level + 1);
                effect.IsLoading = true;
            }
            AllEffects.UnionWith(Effects);
        }

        [UsedImplicitly]
        private void Update()
        {
            if (GameState.Instance.IsPaused || GameState.Instance.IsGameOver)
            {
                return;
            }

            AllEffects.RemoveWhere(effect => !effect.IsConstant && (effect.Duration.Value -= Time.deltaTime) <= 0f);

            var statusColors = new SortedDictionary<string, Color>();
            foreach (var effect in AllEffects)
            {
                if (effect.UpdateTimer(Time.deltaTime) && effect.Source is TowerBase tower)
                {
                    var damage = effect switch
                    {
                        PoisonEffect _ => effect.Amount.Value,
                        BleedEffect _ => Health * effect.Amount.Value / 100f,
                        _ => 0f
                    };

                    tower.EnemyAttacked(Math.Min(damage, Health));
                    if (UpdateHealth(-damage))
                    {
                        tower.EnemyKilled(this);
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
            AllEffects.UnionWith(effects);
            var healthDelta = damage * DamageReduction * DamageTypeReduction(tower.DamageType);
            tower.EnemyAttacked(Math.Min(healthDelta, Health));
            return UpdateHealth(-healthDelta);
        }

        private bool UpdateHealth(float delta)
        {
            if (Health > 0f)
            {
                var oldHealth = Health;
                var newHealth = Health + delta;

                if (Splits > 1 && Math.Ceiling(oldHealth / _splitHealth) > Math.Ceiling(newHealth / _splitHealth))
                {
                    var enemy = Instantiate(gameObject, transform.position, Quaternion.identity, transform.parent);
                    enemy.transform.localScale /= 1.5f;
                    enemy.GetComponentInChildren<Move>().CurrWaypoint = GetComponentInChildren<Move>().CurrWaypoint;

                    var enemyBase = enemy.GetComponent<EnemyBase>();
                    enemyBase.MaxHealth.Base /= Splits * 2;
                    enemyBase.Speed *= 1.5f;
                    enemyBase.Splits /= 2;
                    enemyBase.Level = _level;

                    GameState.Instance.RegisterEnemy(enemy);
                }

                Health = newHealth;
                _healthBar.localScale = new Vector2(Math.Max(Health / MaxHealth.Value, 0f), 1f);
                if (Health <= 0f)
                {
                    OnDie?.Invoke(this, gameObject);
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

        public void AddEffect(EffectBase effect)
        {
            AllEffects.Add(effect);
        }

        public void RemoveEffect(EffectBase effect)
        {
            AllEffects.Remove(effect);
        }
    }
}
