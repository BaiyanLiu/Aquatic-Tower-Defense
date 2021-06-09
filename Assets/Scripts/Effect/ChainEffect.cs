using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Effect
{
    public class ChainEffect : InnateEffect
    {
        public float Range;
        public float RangeGain;

        public override string Name => "Chain Effect";
        public override string AmountName => "Damage";
        public override Color StatusColor => new Color32(255, 242, 0, 255);

        public override void LevelUp()
        {
            base.LevelUp();
            Range += RangeGain;
        }

        public override List<string> GetAmountDisplayText(bool includeGain)
        {
            var amountDisplayText = base.GetAmountDisplayText(includeGain);
            amountDisplayText.Add(FormatAmountDisplayText("Range", Range, RangeGain, includeGain));
            return amountDisplayText;
        }
    }
}
