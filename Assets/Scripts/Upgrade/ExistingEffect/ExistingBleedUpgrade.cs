using System.Collections.Generic;
using Assets.Scripts.Effect;
using JetBrains.Annotations;

namespace Assets.Scripts.Upgrade.ExistingEffect
{
    [UsedImplicitly]
    public sealed class ExistingBleedUpgrade : ExistingEffectUpgrade<BleedEffect>
    {
        public float[] Duration;
        public float[] Frequency;
        
        protected override string AmountName => "Damage";
        protected override string AmountUnit => "%";

        protected override void OnLevelUp()
        {
            var delta = GetDelta(Amount);
            Effect.Amount.Base += delta;
            Effect.Amount.Value += delta;

            delta = GetDelta(Duration);
            Effect.Duration.Base += delta;
            Effect.Duration.Value += delta;

            delta = GetDelta(Frequency);
            Effect.Frequency.Base += delta;
            Effect.Frequency.Value += delta;
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
