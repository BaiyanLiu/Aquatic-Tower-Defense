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
        public Attribute<float> Duration;
        public Attribute<float> Frequency;
        public Attribute<float> Amount;

        public TowerBase Tower { get; set; }
        public ItemBase Item { get; set; }

        public abstract string Name { get; }
        protected virtual string AmountName => "Amount";
        public virtual Color StatusColor => Color.white;
        public virtual bool IsInnate => false;

        private float _effectTimer;

        public static string FormatDisplayText<T>(string name, Attribute<T> attribute, bool includeGain)
        {
            return $"{name}: {attribute.Value}" + (includeGain ? $" (+{attribute.Gain})" : "");
        }

        [UsedImplicitly]
        private void Start()
        {
            Duration.Value = Duration.Base;
            Frequency.Value = Frequency.Base;
            Amount.Value = Amount.Base;
            OnStart();
        }

        protected virtual void OnStart() {}

        public virtual void LevelUp()
        {
            Duration.Value += Duration.Gain;
            Frequency.Value += Frequency.Gain;
            Amount.Value += Amount.Gain;
        }

        public virtual void UpdateLevel(int level)
        {
            Duration.Value = Duration.Base + Duration.Gain * (level - 1);
            Frequency.Value = Frequency.Base + Frequency.Gain * (level - 1);
            Amount.Value = Amount.Base + Amount.Gain * (level - 1);
        }

        public bool UpdateTimer(float deltaTime)
        {
            _effectTimer -= deltaTime;
            if (_effectTimer <= 0f)
            {
                _effectTimer = Frequency.Value;
                return true;
            }
            return false;
        }

        public virtual List<string> GetAmountDisplayText(bool includeGain)
        {
            return new List<string> {FormatDisplayText(AmountName, Amount, includeGain)};
        }

        public object Clone()
        {
            var clone = MemberwiseClone();
            ((EffectBase) clone)._effectTimer = Frequency.Value;
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
