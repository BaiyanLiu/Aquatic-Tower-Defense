using System.Collections.Generic;
using Assets.Scripts.Effect.Innate;

namespace Assets.Scripts.Effect.enemy
{
    public abstract class EnemyEffect : EffectBase
    {
        public Attribute<float> Range;

        protected override void OnStart()
        {
            Range ??= new Attribute<float>();
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
