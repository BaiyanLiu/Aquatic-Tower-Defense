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
        public bool IncludeGain { get; set; } = true;

        public abstract string Name { get; }
        protected virtual string AmountName => "Amount";
        protected virtual string AmountUnit => "";
        public virtual Color StatusColor => Color.white;
        public virtual bool IsInnate => false;

        public bool IsLoaded { private get; set; }

        private float _effectTimer;

        [UsedImplicitly]
        private void Start()
        {
            if (IsLoaded)
            {
                return;
            }

            Duration ??= new Attribute<float>();
            Frequency ??= new Attribute<float>();
            Amount ??= new Attribute<float>();

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

        public virtual List<string> GetAmountDisplayText()
        {
            return new List<string> {FormatDisplayText(AmountName, Amount)};
        }

        public string FormatDisplayText<T>(string attributeName, Attribute<T> attribute)
        {
            return $"{attributeName}: {attribute.Value}{AmountUnit}" + (IncludeGain ? $" (+{attribute.Gain}{AmountUnit})" : "");
        }

        public virtual object Clone()
        {
            var clone = (EffectBase) MemberwiseClone();
            clone._effectTimer = Frequency.Value;
            clone.Duration = (Attribute<float>) Duration.Clone();
            clone.Frequency = (Attribute<float>) Frequency.Clone();
            clone.Amount = (Attribute<float>) Amount.Clone();
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
