using System.Collections.Generic;
using Assets.Scripts.Effect.Innate;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Upgrade.Effect
{
    [UsedImplicitly]
    public sealed class ExperienceTrickleUpgrade : EffectUpgrade<ExperienceTrickleEffect>
    {
        public float[] Frequency;

        protected override void OnLevelUp()
        {
            base.OnLevelUp();
            Effect.Frequency.Value = Frequency[Level];
        }

        public override List<string> GetAmountDisplayText()
        {
            var amountDisplayText = base.GetAmountDisplayText();
            amountDisplayText.Insert(0, FormatDisplayText(Frequency, false));
            return amountDisplayText;
        }

        public override List<Sprite> GetAmountIcon()
        {
            var amountIcon = base.GetAmountIcon();
            amountIcon.Insert(0, Icons.Frequency);
            return amountIcon;
        }
    }
}
