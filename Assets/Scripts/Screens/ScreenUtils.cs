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

        internal static Vector2 UpdateText(Text text, bool isEnabled, Vector2 position, string textValue)
        {
            var parent = text.transform.parent;
            parent.gameObject.SetActive(isEnabled);
            if (isEnabled)
            {
                parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(position.x, -(position.y + Margin));
                text.text = textValue;
                return NextPosition(text, position);
            }
            return position;
        }

        internal static Vector2 NextPosition(Text text, Vector2 position)
        {
            var parent = text.transform.parent;
            var maxWidth = parent.parent.GetComponent<RectTransform>().rect.width;
            var parentRect = parent.GetComponent<RectTransform>().rect;
            var nextColumn = NextColumn(parentRect, position);
            return nextColumn.x >= maxWidth ? NextRow(parentRect, position) : nextColumn;
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
