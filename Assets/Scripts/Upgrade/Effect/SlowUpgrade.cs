using System.Collections.Generic;
using Assets.Scripts.Effect;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Upgrade.Effect
{
    [UsedImplicitly]
    public sealed class SlowUpgrade : EffectUpgrade<SlowEffect>
    {
        public float[] Duration;

        protected override void OnLevelUp()
        {
            base.OnLevelUp();
            Effect.Duration.Value = Duration[Level];
        }

        public override List<string> GetAmountDisplayText()
        {
            var amountDisplayText = base.GetAmountDisplayText();
            amountDisplayText.Insert(0, FormatDisplayText(Duration, false));
            return amountDisplayText;
        }

        public override List<Sprite> GetAmountIcon()
        {
            var amountIcon = base.GetAmountIcon();
            amountIcon.Insert(0, Icons.Duration);
            return amountIcon;
        }
    }
}
