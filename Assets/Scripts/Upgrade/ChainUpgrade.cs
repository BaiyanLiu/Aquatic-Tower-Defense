using System.Collections.Generic;
using Assets.Scripts.Effect;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Upgrade
{
    [UsedImplicitly]
    public sealed class ChainUpgrade : EffectUpgrade<ChainEffect>
    {
        public float[] Range;

        public override string Name => "Chain Upgrade";
        protected override string AmountName => "Damage";
        public override Color TitleColor => new Color32(255, 242, 0, 255);

        protected override void OnLevelUp()
        {
            base.OnLevelUp();
            Effect.Amount.Value = Amount[Level];
            Effect.Range.Value = Range[Level];
        }

        public override List<string> GetAmountDisplayText()
        {
            var amountDisplayText = base.GetAmountDisplayText();
            amountDisplayText.Add(FormatDisplayText("Range", Range, false));
            return amountDisplayText;
        }
    }
}
