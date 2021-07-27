using UnityEngine;

namespace Assets.Scripts.Screens
{
    internal static class ScreenUtils
    {
        public const float Margin = 5f;

        internal static Vector2 UpdateText(IconText text, bool isEnabled, Vector2 position, string textValue, float xOffset = 0f)
        {
            text.gameObject.SetActive(isEnabled);
            if (isEnabled)
            {
                var transform = text.GetComponent<RectTransform>();
                transform.anchoredPosition = new Vector2(position.x + xOffset, -(position.y + Margin));
                text.Text.text = textValue;
                return NextPosition(transform, position);
            }
            return position;
        }

        internal static Vector2 NextPosition(RectTransform transform, Vector2 position)
        {
            var maxWidth = transform.parent.GetComponent<RectTransform>().rect.width;
            var nextColumn = NextColumn(transform.rect, position);
            return nextColumn.x + transform.rect.width > maxWidth ? NextRow(transform.rect, position) : nextColumn;
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
