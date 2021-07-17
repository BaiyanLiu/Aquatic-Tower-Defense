using System.Collections.Generic;
using Assets.Scripts.Effect.Innate;
using JetBrains.Annotations;

namespace Assets.Scripts.Upgrade.Effect
{
    [UsedImplicitly]
    public sealed class ExperienceTrickleUpgrade : EffectUpgrade<ExperienceTrickleEffect>
    {
        public float[] Frequency;

        protected override string AmountName => "Experience";

        protected override void OnLevelUp()
        {
            base.OnLevelUp();
            Effect.Frequency.Value = Frequency[Level];
        }

        public override List<string> GetAmountDisplayText()
        {
            var amountDisplayText = base.GetAmountDisplayText();
            amountDisplayText.Insert(1, FormatDisplayText("Frequency", Frequency, false));
            return amountDisplayText;
        }
    }
}
