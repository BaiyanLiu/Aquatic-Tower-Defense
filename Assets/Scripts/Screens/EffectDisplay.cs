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
        public Text DurationText;
        public Text FrequencyText;
        public Text[] AmountTexts;
        public GameObject[] AmountIcons;

        private SpriteRenderer[] _amountSpriteRenderers;

        [UsedImplicitly]
        private void Start()
        {
            var parent = transform.parent;
            var canvas = parent.GetComponentInParent<Canvas>();
            foreach (var spriteRenderer in parent.GetComponentsInChildren<SpriteRenderer>())
            {
                spriteRenderer.sortingOrder = canvas.sortingOrder;
            }

            _amountSpriteRenderers = AmountIcons.Select(amountIcon => amountIcon.GetComponent<SpriteRenderer>()).ToArray();
        }

        public float UpdateEffect(EffectBase effect)
        {
            NameText.text = effect.Name;
            NameText.color = effect.StatusColor;
            var position = new Vector2(0f, NameText.rectTransform.rect.height);

            position = ScreenUtils.UpdateText(DurationText, effect.Duration.Value > 0f, position, effect.FormatDisplayText(effect.Duration, false));
            position = ScreenUtils.UpdateText(FrequencyText, effect.Frequency.Value > 0f, position, effect.FormatDisplayText(effect.Frequency, false));
            
            using var amountDisplayText = effect.GetAmountDisplayText().GetEnumerator();
            position = AmountTexts.Aggregate(position, (currentPosition, amountText) => ScreenUtils.UpdateText(amountText, amountDisplayText.MoveNext(), currentPosition, amountDisplayText.Current));

            var amountIcon = effect.GetAmountIcon();
            for (var i = 0; i < amountIcon.Count; i++)
            {
                _amountSpriteRenderers[i].sprite = amountIcon[i];
            }

            return ScreenUtils.NextPosition(AmountTexts.Last(), position).y;
        }
    }
}
