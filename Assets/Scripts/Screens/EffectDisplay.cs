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
        public Text AmountText1;
        public Text AmountText2;

        public float UpdateEffect(EffectBase effect)
        {
            NameText.text = effect.Name;
            NameText.color = effect.StatusColor;
            var height = NameText.rectTransform.rect.height;

            height = UpdateText(DurationText, effect.Duration > 0f, height, $"Duration: {effect.Duration} (+{effect.DurationGain})");
            height = UpdateText(FrequencyText, effect.Frequency > 0f, height, $"Frequency: {effect.Frequency} ({effect.FrequencyGain})");

            string amountTextValue1 = null;
            string amountTextValue2 = null;

            switch (effect)
            {
                case PoisonEffect poisonEffect:
                    amountTextValue1 = $"Damage: {poisonEffect.Damage} (+{poisonEffect.DamageGain})";
                    break;

                case SlowEffect slowEffect:
                    amountTextValue1 = $"Amount: {slowEffect.Amount} (+{slowEffect.AmountGain})";
                    break;

                case ChainEffect chainEffect:
                    amountTextValue1 = $"Damage: {chainEffect.Damage} (+{chainEffect.DamageGain})";
                    amountTextValue2 = $"Range: {chainEffect.Range} (+{chainEffect.RangeGain})";
                    break;

                case SplashEffect splashEffect:
                    amountTextValue1 = $"Range: {splashEffect.Range} (+{splashEffect.RangeGain})";
                    break;
            }

            height = UpdateText(AmountText1, amountTextValue1 != null, height, amountTextValue1);
            height = UpdateText(AmountText2, amountTextValue2 != null, height, amountTextValue2);

            return height;
        }

        private float UpdateText(Text text, bool isEnabled, float y, string textValue)
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
