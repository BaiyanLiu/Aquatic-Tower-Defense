using System;
using System.Collections.Generic;
using Assets.Scripts.Tower;
using UnityEngine;

namespace Assets.Scripts.Effect
{
    public abstract class EffectBase : MonoBehaviour, ICloneable, IEqualityComparer<EffectBase>
    {
        public float Duration;
        public float Frequency;
        public float Amount;

        public float DurationGain;
        public float FrequencyGain;
        public float AmountGain;

        public TowerBase Tower { get; set; }

        public abstract string Name { get; }
        public virtual string AmountName => "Amount";
        public virtual Color StatusColor => Color.white;
        public virtual bool IsInnate => false;

        private float _effectTimer;

        public static string FormatAmountDisplayText(string name, float amount, float amountGain, bool includeGain)
        {
            return $"{name}: {amount}" + (includeGain ? $" (+{amountGain})" : "");
        }

        public virtual void LevelUp()
        {
            Duration += DurationGain;
            Frequency += FrequencyGain;
            Amount += AmountGain;
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
            return new List<string> {FormatAmountDisplayText(AmountName, Amount, AmountGain, includeGain)};
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
