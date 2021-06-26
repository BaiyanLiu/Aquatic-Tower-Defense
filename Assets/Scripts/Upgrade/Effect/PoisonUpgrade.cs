using System.Collections.Generic;
using Assets.Scripts.Effect;
using JetBrains.Annotations;

namespace Assets.Scripts.Upgrade.Effect
{
    [UsedImplicitly]
    public sealed class PoisonUpgrade : EffectUpgrade<PoisonEffect>
    {
        public float[] Duration;
        public float[] Frequency;

        public override string Name => "Poison Upgrade";
        protected override string AmountName => "Damage";

        protected override void OnLevelUp()
        {
            base.OnLevelUp();
            Effect.Amount.Value = Amount[Level];
            Effect.Duration.Value = Duration[Level];
            Effect.Frequency.Value = Frequency[Level];
        }

        public override List<string> GetAmountDisplayText()
        {
            var amountDisplayText = base.GetAmountDisplayText();
            amountDisplayText.Insert(0, FormatDisplayText("Duration", Duration, false));
            amountDisplayText.Insert(1, FormatDisplayText("Frequency", Frequency, false));
            return amountDisplayText;
        }
    }
}
