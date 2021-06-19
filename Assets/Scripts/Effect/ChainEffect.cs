using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Effect
{
    public class ChainEffect : InnateEffect
    {
        public float RangeBase;
        public float RangeGain;

        public float Range { get; private set; }

        public override string Name => "Chain Effect";
        protected override string AmountName => "Damage";
        public override Color StatusColor => new Color32(255, 242, 0, 255);

        protected override void OnStart()
        {
            Range = RangeBase;
        }

        public override void LevelUp()
        {
            base.LevelUp();
            Range += RangeGain;
        }

        public override void UpdateLevel(int level)
        {
            base.UpdateLevel(level);
            Range = RangeBase + RangeGain * (level - 1);
        }

        public override List<string> GetAmountDisplayText(bool includeGain)
        {
            var amountDisplayText = base.GetAmountDisplayText(includeGain);
            amountDisplayText.Add(FormatDisplayText("Range", Range, RangeGain, includeGain));
            return amountDisplayText;
        }
    }
}
