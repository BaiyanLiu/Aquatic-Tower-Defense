using System.Collections.Generic;
using Assets.Scripts.Effect.Innate;
using JetBrains.Annotations;

namespace Assets.Scripts.Upgrade.Effect
{
    [UsedImplicitly]
    public sealed class ChainUpgrade : EffectUpgrade<ChainEffect>
    {
        public float[] Range;

        protected override string AmountName => "Damage";

        protected override void OnLevelUp()
        {
            base.OnLevelUp();
            Effect.Range.Value = Range[Level];
        }

        protected override void InitForLoading()
        {
            base.InitForLoading();
            Effect.Range = new AttributeValue();
        }

        public override List<string> GetAmountDisplayText()
        {
            var amountDisplayText = base.GetAmountDisplayText();
            amountDisplayText.Add(FormatDisplayText("Range", Range, false));
            return amountDisplayText;
        }
    }
}
