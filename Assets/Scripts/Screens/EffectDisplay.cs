using System.Linq;
using Assets.Scripts.Effect;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Screens
{
    public sealed class EffectDisplay : MonoBehaviour
    {
        public Text NameText;
        public Text DurationText;
        public Text FrequencyText;
        public Text[] AmountTexts;

        public float UpdateEffect(EffectBase effect)
        {
            NameText.text = effect.Name;
            NameText.color = effect.StatusColor;
            var height = NameText.rectTransform.rect.height;

            height = ScreenUtils.UpdateText(DurationText, effect.Duration.Value > 0f, 0f, height, effect.FormatDisplayText("Duration", effect.Duration));
            height = ScreenUtils.UpdateText(FrequencyText, effect.Frequency.Value > 0f, 0f, height, effect.FormatDisplayText("Frequency", effect.Frequency));

            using var amountDisplayText = effect.GetAmountDisplayText().GetEnumerator();
            height = AmountTexts.Aggregate(height, (currentHeight, amountText) => ScreenUtils.UpdateText(amountText, amountDisplayText.MoveNext(), 0f, currentHeight, amountDisplayText.Current));

            return height;
        }
    }
}
