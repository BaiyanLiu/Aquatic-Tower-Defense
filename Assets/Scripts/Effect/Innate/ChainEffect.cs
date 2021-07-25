using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Effect.Innate
{
    public sealed class ChainEffect : InnateEffect
    {
        public AttributeValue Range;
        
        public override string Name => "Chain Attack";
        protected override Sprite AmountIcon => GameState.Instance.IconsByName["Icon_Dagger"];
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
            amountDisplayText.Add(FormatDisplayText(Range));
            return amountDisplayText;
        }

        public override List<Sprite> GetAmountIcon()
        {
            var amountIcon = base.GetAmountIcon();
            amountIcon.Add(GameState.Instance.IconsByName["Icon_Radar"]);
            return amountIcon;
        }

        public override object Clone()
        {
            var clone = (ChainEffect) base.Clone();
            clone.Range = (AttributeValue) Range.Clone();
            return clone;
        }
    }
}
