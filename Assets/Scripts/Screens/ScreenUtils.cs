using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Screens
{
    internal static class ScreenUtils
    {
        private const float Margin = 5f;

        internal static float UpdateText(Text text, bool isEnabled, float x, float y, string textValue)
        {
            if (isEnabled)
            {
                text.rectTransform.anchoredPosition = new Vector2(x, -(y + Margin));
                text.text = textValue;
                return y + text.rectTransform.rect.height + Margin;
            }
            text.text = null;
            return y;
        }

        internal static Vector2 UpdateText(IconText text, bool isEnabled, Vector2 position, string textValue)
        {
            text.gameObject.SetActive(isEnabled);
            if (isEnabled)
            {
                var transform = text.GetComponent<RectTransform>();
                transform.anchoredPosition = new Vector2(position.x, -(position.y + Margin));
                text.Text.text = textValue;
                return NextPosition(transform, position);
            }
            return position;
        }

        internal static Vector2 NextPosition(RectTransform transform, Vector2 position)
        {
            var maxWidth = transform.parent.GetComponent<RectTransform>().rect.width;
            var nextColumn = NextColumn(transform.rect, position);
            return nextColumn.x >= maxWidth ? NextRow(transform.rect, position) : nextColumn;
        }

        private static Vector2 NextColumn(Rect rect, Vector2 position)
        {
            return new Vector2(position.x + rect.width, position.y);
        }

        private static Vector2 NextRow(Rect rect, Vector2 position)
        {
            return new Vector2(0f, position.y + rect.height + Margin);

        }
    }
}
