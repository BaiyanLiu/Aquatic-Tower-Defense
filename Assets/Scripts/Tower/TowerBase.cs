using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Effect;
using Assets.Scripts.Effect.Innate;
using Assets.Scripts.Enemy;
using Assets.Scripts.Item;
using Assets.Scripts.Upgrade;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    public sealed class TowerBase : MonoBehaviour, IHasItems
    {
        public event EventHandler<ItemBase> OnItemAdded;
        public event EventHandler<int> OnItemRemoved;

        public Attribute<float> Damage;
        public Attribute<float> Range;
        public Attribute<float> AttackSpeed;
        public Attribute<float> ProjectileSpeed;

        public DamageType DamageType;
        public int Cost;
        public string Name;

        public int Level { get; set; } = 1;
        public int Experience { get; set; }
        public int ExperienceRequired { get; set; } = 100;

        public float DamageDone { get; set; }
        public int Kills { get; set; }
        public Attribute<float> SellCost { get; } = new Attribute<float>();

        public List<EffectBase> Effects { get; } = new List<EffectBase>();
        public List<EffectBase> AllEffects { get; } = new List<EffectBase>();
        public List<ItemBase> Items { get; } = new List<ItemBase>();
        public bool IsInventoryFull => Items.Count == 6;
        public UpgradeBase[] Upgrades { get; private set; }

        private GameState _gameState;
        private CircleCollider2D _collider;

        [UsedImplicitly]
        private void Start()
        {
            _gameState = GameState.GetGameState(gameObject);
            _collider = GetComponent<CircleCollider2D>();

            SellCost.Value = SellCost.Base = Cost / 2f;
            SellCost.Gain = SellCost.Base / 10f;

            Effects.AddRange(GetComponents<EffectBase>());
            Effects.ForEach(effect => effect.Tower = this);
            AllEffects.AddRange(Effects);

            Upgrades = GetComponents<UpgradeBase>();
            foreach (var upgrade in Upgrades)
            {
                upgrade.Tower = this;
            }

            UpdateStats();
        }

        public void EnemyAttacked(float damage)
        {
            DamageDone += Math.Max(damage, 0f);
        }

        public void EnemyKilled(EnemyBase enemy)
        {
            UpdateExperience(enemy.Experience);
            Kills++;
            _gameState.EnemyKilled(this, enemy);
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
            }
        }

        public void UpdateStats()
        {
            Damage.Value = Damage.Base + Damage.Gain * (Level - 1);
            Range.Value = Range.Base + Range.Gain * (Level - 1);
            AttackSpeed.Value = AttackSpeed.Base + AttackSpeed.Gain * (Level - 1);
            ProjectileSpeed.Value = ProjectileSpeed.Base + ProjectileSpeed.Gain * (Level - 1);
            SellCost.Value = SellCost.Base + SellCost.Gain * (Level - 1);

            foreach (var effect in Items.SelectMany(item => item.Effects))
            {
                if (effect is DamageEffect)
                {
                    Damage.Value += effect.Amount.Value;
                }
            }

            foreach (var upgrade in Upgrades)
            {
                upgrade.Apply();
            }

            SellCost.Value = (float) Math.Round(SellCost.Value);

            _collider.radius = Range.Value;
        }

        public void AddItem(ItemBase item)
        {
            Items.Add(item);
            AllEffects.AddRange(item.Effects);
            item.UpdateTower(this);
            UpdateStats();
            OnItemAdded?.Invoke(this, item);
        }

        public void RemoveItem(int index)
        {
            Items[index].UpdateTower(null);
            AllEffects.RemoveAll(effect => effect.Item == Items[index]);
            Items.RemoveAt(index);
            UpdateStats();
            OnItemRemoved?.Invoke(this, index);
        }
    }
}
