using System.Collections.Generic;
using Assets.Scripts.Effect;
using JetBrains.Annotations;

namespace Assets.Scripts.Upgrade.Effect
{
    [UsedImplicitly]
    public sealed class SlowUpgrade : EffectUpgrade<SlowEffect>
    {
        public float[] Duration;

        public override string Name => "Slow Upgrade";

        protected override void OnLevelUp()
        {
            base.OnLevelUp();
            Effect.Amount.Value = Amount[Level];
            Effect.Duration.Value = Duration[Level];
        }

        public override List<string> GetAmountDisplayText()
        {
            var amountDisplayText = base.GetAmountDisplayText();
            amountDisplayText.Insert(0, FormatDisplayText("Duration", Duration, false));
            return amountDisplayText;
        }
    }
}
