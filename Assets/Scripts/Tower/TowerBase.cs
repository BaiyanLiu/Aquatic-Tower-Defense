using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Effect;
using Assets.Scripts.Effect.Area;
using Assets.Scripts.Effect.Innate;
using Assets.Scripts.Effect.Innate.Attribute;
using Assets.Scripts.Enemy;
using Assets.Scripts.Item;
using Assets.Scripts.Persistence;
using Assets.Scripts.Upgrade;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    public sealed class TowerBase : MonoBehaviour, IHasItems, IHasEffect
    {
        public event EventHandler<ItemBase> OnItemAdded;
        public event EventHandler<int> OnItemRemoved;
        public event EventHandler OnLevelUp; 
        public event EventHandler<GameObject> OnDestroyed;

        public AttributeValue Damage;
        public AttributeValue Range;
        public AttributeValue AttackSpeed;
        public AttributeValue ProjectileSpeed;

        public DamageType DamageType;
        public int Cost;
        public string Name;

        public int Level { get; set; } = 1;
        public int Experience { get; set; }
        public int ExperienceRequired { get; set; } = 100;

        public float DamageDone { get; set; }
        public int Kills { get; set; }
        public AttributeValue SellCost { get; } = new AttributeValue();

        public List<EffectBase> Effects { get; } = new List<EffectBase>();
        public HashSet<EffectBase> AllEffects { get; } = new HashSet<EffectBase>();
        public List<ItemBase> Items { get; } = new List<ItemBase>();
        public bool IsInventoryFull => Items.Count == 6;
        public UpgradeBase[] Upgrades { get; private set; }

        private CircleCollider2D _collider;
        private bool _isLoading;
        private int[] _upgradeLevels;

        [UsedImplicitly]
        private void Start()
        {
            _collider = GetComponent<CircleCollider2D>();

            if (!_isLoading)
            {
                SellCost.Base = Cost / 2f;
                SellCost.Gain = SellCost.Base / 10f;
            }
            SellCost.Value = SellCost.Base;

            Effects.AddRange(GetComponents<EffectBase>());
            Effects.ForEach(effect =>
            {
                effect.Source = this;
                effect.UpdateLevel(Level);
                effect.IsLoading = _isLoading;
            });
            AllEffects.UnionWith(Effects);

            Upgrades = GetComponents<UpgradeBase>();
            for (var i = 0; i < Upgrades.Length; i++)
            {
                Upgrades[i].Tower = this;
                if (_isLoading && _upgradeLevels.Any())
                {
                    Upgrades[i].Level = _upgradeLevels[i];
                    Upgrades[i].IsLoading = true;
                }
            }

            if (_isLoading)
            {
                Items.ForEach(item =>
                {
                    AllEffects.UnionWith(item.Effects);
                    item.UpdateTower(this);
                });
            }

            UpdateStats();
        }

        [UsedImplicitly]
        private void Update()
        {
            if (GameState.Instance.IsPaused || GameState.Instance.IsGameOver)
            {
                return;
            }

            foreach (var effect in AllEffects.Where(effect => effect.UpdateTimer(Time.deltaTime)))
            {
                if (GameState.Instance.IsWaveActive)
                {
                    switch (effect)
                    {
                        case ExperienceTrickleEffect _:
                            UpdateExperience((int) effect.Amount.Value);
                            break;
                        case GoldTrickleEffect _:
                            GameState.Instance.UpdateGold((int) effect.Amount.Value, this);
                            break;
                    }
                    
                }
            }
        }

        [UsedImplicitly]
        private void OnDestroy()
        {
            OnDestroyed?.Invoke(this, transform.parent.gameObject);
        }

        public void EnemyAttacked(float damage)
        {
            DamageDone += Math.Max(damage, 0f);
        }

        public void EnemyKilled(EnemyBase enemy)
        {
            UpdateExperience(enemy.Experience);
            Kills++;
            GameState.Instance.EnemyKilled(this, enemy);
        }

        public void UpdateExperience(int delta)
        {
            var experienceAmount = AllEffects
                .Where(effect => effect is ExperienceEffect)
                .Select(effect => effect.Amount.Value)
                .Prepend(100f)
                .Max();

            Experience += (int) (delta * experienceAmount / 100f);
            if (Experience >= ExperienceRequired)
            {
                Level++;
                Experience -= ExperienceRequired;
                ExperienceRequired += 100;

                foreach (var effect in Effects)
                {
                    effect.LevelUp();
                }

                UpdateStats();
                OnLevelUp?.Invoke(this, EventArgs.Empty);
            }
        }

        public void UpdateStats()
        {
            Damage.Value = Damage.Base + Damage.Gain * (Level - 1);
            Range.Value = Range.Base + Range.Gain * (Level - 1);
            AttackSpeed.Value = AttackSpeed.Base + AttackSpeed.Gain * (Level - 1);
            ProjectileSpeed.Value = ProjectileSpeed.Base + ProjectileSpeed.Gain * (Level - 1);
            SellCost.Value = SellCost.Base + SellCost.Gain * (Level - 1);

            var attributeEffects = Items.SelectMany(item => item.Effects).OfType<AttributeEffect>().ToArray();
            foreach (var effect in attributeEffects.Where(effect => !effect.IsMultiply))
            {
                effect.Apply();
            }

            foreach (var upgrade in Upgrades)
            {
                upgrade.Apply();
            }

            var damageAmounts = AllEffects.OfType<AreaDamageEffect>()
                .Where(effect => effect.Amount != null)
                .Select(effect => effect.Amount.Value)
                .ToArray();
            var damageIncreaseAmount = damageAmounts.Where(amount => amount > 0f).Prepend(0f).Max();
            var damageDecreaseAmount = damageAmounts.Where(amount => amount < 0f).Prepend(0f).Min();
            Damage.Value *= 1f + (damageIncreaseAmount + damageDecreaseAmount) / 100f;

            var attackSpeedIncreaseAmount = AllEffects.OfType<AreaAttackSpeedEffect>().Select(effect => effect.Amount.Value).Prepend(0f).Max();
            AttackSpeed.Value /= 1f + attackSpeedIncreaseAmount / 100f;

            foreach (var effect in attributeEffects.Where(effect => effect.IsMultiply))
            {
                effect.Apply();
            }

            SellCost.Value = (float) Math.Round(SellCost.Value);

            _collider.radius = Range.Value;
        }

        public void AddItem(ItemBase item)
        {
            Items.Add(item);
            AllEffects.UnionWith(item.Effects);
            item.UpdateTower(this);
            UpdateStats();
            OnItemAdded?.Invoke(this, item);
        }

        public void RemoveItem(int index)
        {
            Items[index].UpdateTower(null);
            AllEffects.RemoveWhere(effect => effect.Item == Items[index]);
            Items.RemoveAt(index);
            UpdateStats();
            OnItemRemoved?.Invoke(this, index);
        }

        public TowerSnapshot ToSnapshot()
        {
            return new TowerSnapshot
            {
                Name = Name,
                Position = transform.position,
                Level = Level,
                Experience = Experience,
                ExperienceRequired = ExperienceRequired,
                DamageDone = DamageDone,
                Kills = Kills,
                SellCost = SellCost,
                Items = Items.Select(item => item.ToSnapshot()).ToArray(),
                Upgrades = Upgrades?.Select(upgrade => upgrade.Level).ToArray()
            };
        }

        public static GameObject FromSnapshot(TowerSnapshot snapshot)
        {
            var tower = Instantiate(GameState.Instance.TowersByName[snapshot.Name], snapshot.Position, Quaternion.identity);

            var towerBase = tower.GetComponentInChildren<TowerBase>();
            towerBase.Level = snapshot.Level;
            towerBase.Experience = snapshot.Experience;
            towerBase.ExperienceRequired = snapshot.ExperienceRequired;
            towerBase.DamageDone = snapshot.DamageDone;
            towerBase.Kills = snapshot.Kills;
            towerBase.SellCost.Base = snapshot.SellCost.Base;
            towerBase.SellCost.Gain = snapshot.SellCost.Gain;

            towerBase.Items.Clear();
            foreach (var item in snapshot.Items)
            {
                towerBase.Items.Add(ItemBase.FromSnapshot(item));
            }

            towerBase._upgradeLevels = snapshot.Upgrades;
            towerBase._isLoading = true;

            return tower;
        }

        public void AddEffect(EffectBase effect)
        {
            AllEffects.Add(effect);
            UpdateStats();
        }

        public void RemoveEffect(EffectBase effect)
        {
            AllEffects.Remove(effect);
            UpdateStats();
        }
    }
}
