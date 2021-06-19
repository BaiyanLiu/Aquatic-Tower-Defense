using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Screens
{
    internal static class ScreenUtils
    {
        internal static float UpdateText(Text text, bool isEnabled, float x, float y, string textValue)
        {
            if (isEnabled)
            {
                text.rectTransform.anchoredPosition = new Vector2(x, -(y + 5f));
                text.text = textValue;
                return y + text.rectTransform.rect.height + 5f;
            }
            text.text = null;
            return y;
        }
    }
}
