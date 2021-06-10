using System.Linq;
using Assets.Scripts.Effect;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Screens
{
    public class EffectDisplay : MonoBehaviour
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

            height = UpdateText(DurationText, effect.Duration > 0f, height, EffectBase.FormatDisplayText("Duration", effect.Duration, effect.DurationGain, IncludeGain));
            height = UpdateText(FrequencyText, effect.Frequency > 0f, height, EffectBase.FormatDisplayText("Frequency", effect.Frequency, effect.FrequencyGain, IncludeGain));

            using var amountDisplayText = effect.GetAmountDisplayText(IncludeGain).GetEnumerator();
            height = AmountTexts.Aggregate(height, (currentHeight, amountText) => UpdateText(amountText, amountDisplayText.MoveNext(), currentHeight, amountDisplayText.Current));

            return height;
        }

        private static float UpdateText(Text text, bool isEnabled, float y, string textValue)
        {
            if (isEnabled)
            {
                text.rectTransform.anchoredPosition = new Vector2(0f, -(y + 5f));
                text.text = textValue;
                return y + text.rectTransform.rect.height + 5f;
            }
            text.text = null;
            return y;
        }
    }
}
