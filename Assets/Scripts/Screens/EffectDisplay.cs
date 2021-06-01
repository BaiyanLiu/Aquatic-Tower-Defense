using Assets.Scripts.Effect;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Screens
{
    public class EffectDisplay : MonoBehaviour
    {
        public Text NameText;
        public Text DurationText;
        public Text FrequencyText;
        public Text AmountText;

        public float UpdateEffect(EffectBase effect)
        {
            NameText.text = effect.Name;
            var height = NameText.rectTransform.rect.height + 5f;

            height = UpdateText(DurationText, effect.Duration > 0f, height, $"Duration: {effect.Duration} (+{effect.DurationGain})");
            height = UpdateText(FrequencyText, effect.Frequency > 0f, height, $"Frequency: {effect.Frequency} ({effect.FrequencyGain})");

            var amountTextValue = effect switch
            {
                PoisonEffect poisonEffect => $"Damage: {poisonEffect.Damage} (+{poisonEffect.DamageGain})",
                SlowEffect slowEffect => $"Amount: {slowEffect.Amount} (+{slowEffect.AmountGain})",
                _ => null
            };
            height = UpdateText(AmountText, true, height, amountTextValue);

            return height - 5f;
        }

        private float UpdateText(Text text, bool isEnabled, float y, string textValue)
        {
            if (isEnabled)
            {
                text.rectTransform.anchoredPosition = new Vector2(0f, -y);
                text.text = textValue;
                return y + text.rectTransform.rect.height + 5f;
            }
            text.text = null;
            return y;
        }
    }
}
