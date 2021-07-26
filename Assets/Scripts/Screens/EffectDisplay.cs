using System.Linq;
using Assets.Scripts.Effect;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Screens
{
    public sealed class EffectDisplay : MonoBehaviour
    {
        public Text NameText;
        public IconText Duration;
        public IconText Frequency;
        public IconText[] Amounts;

        [UsedImplicitly]
        private void Start()
        {
            var sortingOrder = transform.parent.GetComponentInParent<Canvas>().sortingOrder;
            Duration.Icon.sortingOrder = sortingOrder;
            Frequency.Icon.sortingOrder = sortingOrder;
            foreach (var amount in Amounts)
            {
                amount.Icon.sortingOrder = sortingOrder;
            }
        }

        public float UpdateEffect(EffectBase effect)
        {
            NameText.text = effect.Name;
            NameText.color = effect.StatusColor;
            var position = new Vector2(0f, NameText.rectTransform.rect.height);

            position = ScreenUtils.UpdateText(Duration, effect.Duration.Value > 0f, position, effect.FormatDisplayText(effect.Duration, false));
            position = ScreenUtils.UpdateText(Frequency, effect.Frequency.Value > 0f, position, effect.FormatDisplayText(effect.Frequency, false));
            
            using var amountDisplayText = effect.GetAmountDisplayText().GetEnumerator();
            position = Amounts.Aggregate(position, (currentPosition, amount) => ScreenUtils.UpdateText(amount, amountDisplayText.MoveNext(), currentPosition, amountDisplayText.Current));

            var amountIcon = effect.GetAmountIcon();
            for (var i = 0; i < amountIcon.Count; i++)
            {
                Amounts[i].Icon.sprite = amountIcon[i];
            }

            return ScreenUtils.NextPosition(Amounts.Last().GetComponent<RectTransform>(), position).y;
        }
    }
}
