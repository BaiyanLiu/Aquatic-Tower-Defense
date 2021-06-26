using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Effect.Innate
{
    public sealed class ChainEffect : InnateEffect
    {
        public Attribute<float> Range;

        public override string Name => "Chain Effect";
        protected override string AmountName => "Damage";
        public override Color StatusColor => new Color32(255, 242, 0, 255);

        protected override void OnStart()
        {
            Range ??= new Attribute<float>();
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
            clone.Range = (Attribute<float>) Range.Clone();
            return clone;
        }
    }
}
