using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Effect.Area
{
    public abstract class AreaEffect<T> : EffectBase where T : IHasEffect
    {
        public Attribute<float> Range;
        public CircleCollider2D Collider;

        public override bool IsConstant => true;

        protected override void OnStart()
        {
            Range ??= new Attribute<float>();
            Collider.radius = Range.Value = Range.Base;
        }

        public override void LevelUp()
        {
            base.LevelUp();
            Range.Value += Range.Gain;
            Collider.radius = Range.Value;
        }

        public override void UpdateLevel(int level)
        {
            base.UpdateLevel(level);
            Range.Value = Range.Base + Range.Gain * (level - 1);
            Collider.radius = Range.Value;
        }

        public override List<string> GetAmountDisplayText()
        {
            var amountDisplayText = base.GetAmountDisplayText();
            amountDisplayText.Add(FormatDisplayText("Range", Range, false));
            return amountDisplayText;
        }

        public override object Clone()
        {
            var clone = (AreaEffect<T>) base.Clone();
            clone.Range = (Attribute<float>) Range.Clone();
            return clone;
        }

        [UsedImplicitly]
        private void OnTriggerEnter2D(Collider2D collision)
        {
            var target = collision.gameObject.GetComponent<T>();
            if (target != null)
            {
                target.AddEffect(this);
                OnTargetEnter(target);
            }
        }

        protected virtual void OnTargetEnter(T target) {}

        [UsedImplicitly]
        private void OnTriggerExit2D(Collider2D collision)
        {
            var target = collision.gameObject.GetComponent<T>();
            if (target != null)
            {
                target.RemoveEffect(this);
                OnTargetExit(target);
            }
        }

        protected virtual void OnTargetExit(T target) {}
    }
}
