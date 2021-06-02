using System.Collections.Generic;
using Assets.Scripts.Effect;
using Assets.Scripts.Enemy;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class TowerBase : MonoBehaviour
    {
        public float Damage;
        public float Range;
        public float AttackSpeed;
        public float ProjectileSpeed;

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
        public float DamageDone { get; set; }
        public int Kills { get; set; }

        public List<EffectBase> Effects { get; } = new List<EffectBase>();

        private CircleCollider2D _collider;

        private void Start()
        {
            _collider = GetComponent<CircleCollider2D>();
            _collider.radius = Range;

            Effects.AddRange(GetComponents<EffectBase>());
            foreach (var effect in Effects)
            {
                effect.Tower = this;
            }
        }

        public void EnemyAttacked(float damage)
        {
            DamageDone += damage;
        }

        public void EnemyKilled(EnemyBase enemy)
        {
            UpdateExperience(enemy.Experience);
            Kills++;
        }

        public void UpdateExperience(int delta)
        {
            Experience += delta;
            if (Experience >= ExperienceRequired)
            {
                Level++;
                Experience -= ExperienceRequired;
                ExperienceRequired += 100;

                Damage += DamageGain;
                Range += RangeGain;
                AttackSpeed += AttackSpeedGain;
                ProjectileSpeed += ProjectileSpeedGain;

                foreach (var effect in Effects)
                {
                    effect.LevelUp();
                }

                _collider.radius = Range;
            }
        }
    }
}
