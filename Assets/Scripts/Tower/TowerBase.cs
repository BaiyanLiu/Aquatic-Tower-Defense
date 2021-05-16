using System.Collections.Generic;
using Assets.Scripts.Effect;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class TowerBase : MonoBehaviour
    {
        public float Damage;
        public float Range;
        public float AttackSpeed;
        public float ProjectileSpeed;
        public float Splash;
        public float ChainDamage;
        public float ChainRange;

        public float DamageGain;
        public float RangeGain;
        public float AttackSpeedGain;
        public float ProjectileSpeedGain;
        public float SplashGain;
        public float ChainDamageGain;
        public float ChainRangeGain;

        public DamageType DamageType;

        public int Level { get; set; } = 1;
        public int Experience { get; set; }
        public int ExperienceRequired { get; set; } = 100;

        public List<EffectBase> Effects { get; } = new List<EffectBase>();

        private CircleCollider2D _collider;

        private void Start()
        {
            _collider = GetComponent<CircleCollider2D>();
            _collider.radius = Range;

            Effects.AddRange(GetComponents<EffectBase>());
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
                Splash += SplashGain;
                ChainDamage += ChainDamageGain;
                ChainRange += ChainRangeGain;

                foreach (var effect in Effects)
                {
                    effect.LevelUp();
                }

                _collider.radius = Range;
            }
        }
    }
}
