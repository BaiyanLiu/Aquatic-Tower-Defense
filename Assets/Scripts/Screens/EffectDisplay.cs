using System.Linq;
using Assets.Scripts.Effect;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Screens
{
    public sealed class EffectDisplay : MonoBehaviour
    {
        public bool IncludeGain = true;

        public Text NameText;
        public Text DurationText;
        public Text FrequencyText;
        public Text[] AmountTexts;

        public float UpdateEffect(EffectBase effect)
        {
            NameText.text = effect.Name;
            NameText.color = effect.StatusColor;
            var height = NameText.rectTransform.rect.height;

            height = ScreenUtils.UpdateText(DurationText, effect.Duration > 0f, 0f, height, EffectBase.FormatDisplayText("Duration", effect.Duration, effect.DurationGain, IncludeGain));
            height = ScreenUtils.UpdateText(FrequencyText, effect.Frequency > 0f, 0f, height, EffectBase.FormatDisplayText("Frequency", effect.Frequency, effect.FrequencyGain, IncludeGain));

            using var amountDisplayText = effect.GetAmountDisplayText(IncludeGain).GetEnumerator();
            height = AmountTexts.Aggregate(height, (currentHeight, amountText) => ScreenUtils.UpdateText(amountText, amountDisplayText.MoveNext(), 0f, currentHeight, amountDisplayText.Current));

            return height;
        }
    }
}
