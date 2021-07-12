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

        public Attribute<float> MaxHealth;
        public Attribute<float> Armor;

        public float Speed;
        public ArmorType ArmorType;
        public int Experience;
        public int Gold;
        public int Lives;
        public string Name;

        public int Splits;

        public float ItemChance;
        public int ItemLevelBonus;

        public float Health { get; private set; }
        public HashSet<EffectBase> Effects { get; } = new HashSet<EffectBase>();

        private int _level;
        public int Level
        {
            set
            {
                _level = value;
                MaxHealth.Value = (MaxHealth.Base + MaxHealth.Gain * value) * PlayerPrefs.GetInt(Settings.Health) / 100f;
                Armor.Value = Armor.Base + Armor.Gain * value;
                Health = MaxHealth.Value;
            }
        }

        private float DamageReduction => (100f - Armor.Value) / 100f;

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
        }

        [UsedImplicitly]
        private void Update()
        {
            if (GameState.Instance.IsPaused || GameState.Instance.IsGameOver)
            {
                return;
            }

            Effects.RemoveWhere(effect => (effect.Duration.Value -= Time.deltaTime) <= 0f);

            var statusColors = new SortedDictionary<string, Color>();
            foreach (var effect in Effects)
            {
                if (effect.UpdateTimer(Time.deltaTime))
                {
                    if (effect is PoisonEffect)
                    {
                        effect.Tower.EnemyAttacked(Math.Min(effect.Amount.Value, Health));
                        if (UpdateHealth(-effect.Amount.Value))
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
                _healthBar.localScale = new Vector2(Math.Max(Health / MaxHealth.Value, 0f), 1f);
                if (Health <= 0f)
                {

                    if (Splits > 1)
                    {
                        var distX = CalcSplitDist(_animator.GetFloat("DirX"));
                        var distY = CalcSplitDist(_animator.GetFloat("DirY"));

                        for (var i = 0; i < Splits; i++)
                        {
                            var enemy = Instantiate(gameObject, transform.position + new Vector3(distX * i, distY * i, 0f), Quaternion.identity, transform.parent);
                            enemy.transform.localScale /= 1.5f;
                            enemy.GetComponentInChildren<Move>().CurrWaypoint = GetComponentInChildren<Move>().CurrWaypoint;

                            var enemyBase = enemy.GetComponent<EnemyBase>();
                            enemyBase.MaxHealth.Base /= 2f;
                            enemyBase.Level = _level;
                            enemyBase.Splits /= 2;

                            GameState.Instance.RegisterEnemy(enemy);
                        }
                    }

                    OnDie?.Invoke(this, gameObject);
                    GameState.Instance.UpdateGold(Gold);
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

        private static float CalcSplitDist(float dir)
        {
            if (dir > 0.1f)
            {
                return -0.1f;
            }
            else if (dir < -0.1f)
            {
                return 0.1f;
            }
            return 0f;
        }
    }
}
