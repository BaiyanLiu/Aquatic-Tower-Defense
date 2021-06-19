using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Effect;
using Assets.Scripts.Enemy;
using Assets.Scripts.Item;
using Assets.Scripts.Upgrade;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class TowerBase : MonoBehaviour, IHasItems
    {
        public event EventHandler<ItemBase> OnItemAdded;
        public event EventHandler<int> OnItemRemoved;

        public float DamageBase;
        public float RangeBase;
        public float AttackSpeedBase;
        public float ProjectileSpeedBase;

        public float DamageGain;
        public float RangeGain;
        public float AttackSpeedGain;
        public float ProjectileSpeedGain;

        public DamageType DamageType;
        public int Cost;
        public string Name;

        public int Level { get; set; } = 1;
        public int Experience { get; set; }
        public int ExperienceRequired { get; set; } = 100;

        public float Damage { get; private set; }
        public float Range { get; private set; }
        public float AttackSpeed { get; private set; }
        public float ProjectileSpeed { get; private set; }

        public float DamageDone { get; set; }
        public int Kills { get; set; }
        public int SellCost { get; private set; }

        public EffectBase[] Effects { get; private set; }
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

            SellCost = (int) Math.Round(Cost / 2f);

            Effects = GetComponents<EffectBase>();
            foreach (var effect in Effects)
            {
                effect.Tower = this;
            }
            AllEffects.AddRange(Effects);

            Upgrades = GetComponents<UpgradeBase>();

            UpdateStats();
        }

        public void EnemyAttacked(float damage)
        {
            DamageDone += damage;
        }

        public void EnemyKilled(EnemyBase enemy)
        {
            UpdateExperience(enemy.Experience);
            Kills++;
            _gameState.EnemyKilled(this, enemy);
        }

        public void UpdateExperience(int delta)
        {
            Experience += delta;
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
            Damage = DamageBase + DamageGain * (Level - 1);
            Range = RangeBase + RangeGain * (Level - 1);
            AttackSpeed = AttackSpeedBase + AttackSpeedGain * (Level - 1);
            ProjectileSpeed = ProjectileSpeedBase + ProjectileSpeedGain * (Level - 1);

            foreach (var effect in Items.SelectMany(item => item.Effects))
            {
                if (effect is DamageEffect)
                {
                    Damage += effect.Amount;
                }
            }

            _collider.radius = Range;
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
