using System.Collections.Generic;
using Assets.Scripts.Effect;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Upgrade.Effect
{
    [UsedImplicitly]
    public sealed class PoisonUpgrade : EffectUpgrade<PoisonEffect>
    {
        public float[] Duration;
        public float[] Frequency;

        protected override void OnLevelUp()
        {
            base.OnLevelUp();
            Effect.Duration.Value = Duration[Level];
            Effect.Frequency.Value = Frequency[Level];
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
