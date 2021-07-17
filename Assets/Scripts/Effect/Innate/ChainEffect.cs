using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Effect.Innate
{
    public sealed class ChainEffect : InnateEffect
    {
        public AttributeValue Range;

        public override string Name => "Chain Attack";
        protected override string AmountName => "Damage";
        public override Color StatusColor => Colors.Instance.Yellow;

        protected override void OnStart()
        {
            Range ??= new AttributeValue();
            Range.Value = Range.Base;
        }

        public override void LevelUp()
        {
            base.LevelUp();
            Range.Value += Range.Gain;
        }

        public override void UpdateLevel(int level)
        {
            base.UpdateLevel(level);
            Range.Value = Range.Base + Range.Gain * (level - 1);
        }

        public override List<string> GetAmountDisplayText()
        {
            var amountDisplayText = base.GetAmountDisplayText();
            amountDisplayText.Add(FormatDisplayText("Range", Range));
            return amountDisplayText;
        }

        public override object Clone()
        {
            var clone = (ChainEffect) base.Clone();
            clone.Range = (AttributeValue) Range.Clone();
            return clone;
        }
    }
}
