using System;
using System.Collections.Generic;
using Assets.Scripts.Item;
using Assets.Scripts.Tower;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Effect
{
    public abstract class EffectBase : MonoBehaviour, ICloneable, IEqualityComparer<EffectBase>
    {
        public float DurationBase;
        public float FrequencyBase;
        public float AmountBase;

        public float DurationGain;
        public float FrequencyGain;
        public float AmountGain;

        public float Duration { get; set; }
        public float Frequency { get; private set; }
        public float Amount { get; private set; }

        public TowerBase Tower { get; set; }
        public ItemBase Item { get; set; }

        public abstract string Name { get; }
        public virtual string AmountName => "Amount";
        public virtual Color StatusColor => Color.white;
        public virtual bool IsInnate => false;

        private float _effectTimer;

        public static string FormatDisplayText(string name, float amount, float amountGain, bool includeGain)
        {
            return $"{name}: {amount}" + (includeGain ? $" (+{amountGain})" : "");
        }

        [UsedImplicitly]
        private void Start()
        {
            Duration = DurationBase;
            Frequency = FrequencyBase;
            Amount = AmountBase;
            OnStart();
        }

        protected virtual void OnStart() {}

        public virtual void LevelUp()
        {
            Duration += DurationGain;
            Frequency += FrequencyGain;
            Amount += AmountGain;
        }

        public virtual void UpdateLevel(int level)
        {
            Duration = DurationBase + DurationGain * (level - 1);
            Frequency = FrequencyBase + FrequencyGain * (level - 1);
            Amount = AmountBase + AmountGain * (level - 1);
        }

        public bool UpdateTimer(float deltaTime)
        {
            _effectTimer -= deltaTime;
            if (_effectTimer <= 0f)
            {
                _effectTimer = Frequency;
                return true;
            }
            return false;
        }

        public virtual List<string> GetAmountDisplayText(bool includeGain)
        {
            return new List<string> {FormatDisplayText(AmountName, Amount, AmountGain, includeGain)};
        }

        public object Clone()
        {
            var clone = MemberwiseClone();
            ((EffectBase) clone)._effectTimer = Frequency;
            return clone;
        }

        public bool Equals(EffectBase x, EffectBase y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x is null) return false;
            if (y is null) return false;
            return x.GetType() == y.GetType() && Equals(x.Tower, y.Tower);
        }

        public int GetHashCode(EffectBase obj)
        {
            return obj.Tower != null ? obj.Tower.GetHashCode() : 0;
        }
    }
}
