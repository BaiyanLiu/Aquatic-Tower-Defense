using System.Collections.Generic;
using Assets.Scripts.Effect;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Upgrade.ExistingEffect
{
    [UsedImplicitly]
    public sealed class ExistingPoisonUpgrade : ExistingEffectUpgrade<PoisonEffect>
    {
        public float[] Duration;
        public float[] Frequency;
        
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
            amountDisplayText.Insert(0, FormatDisplayText(Duration, false));
            amountDisplayText.Insert(1, FormatDisplayText(Frequency, false));
            return amountDisplayText;
        }

        public override List<Sprite> GetAmountIcon()
        {
            var amountIcon = base.GetAmountIcon();
            amountIcon.Insert(0, Icons.Duration);
            amountIcon.Insert(1, Icons.Frequency);
            return amountIcon;
        }
    }
}
