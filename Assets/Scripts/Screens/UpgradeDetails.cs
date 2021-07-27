using System.Linq;
using Assets.Scripts.Upgrade;
using UnityEngine;

namespace Assets.Scripts.Screens
{
    public sealed class UpgradeDetails : DetailsScreen<UpgradeBase>
    {
        public IconText Cost;
        public IconText[] Amounts;

        protected override float OnUpdate(float height)
        {
            NameText.text = Base.Name;
            NameText.color = Base.NameColor;
            var position = new Vector2(0f, height + NameText.rectTransform.rect.height + ScreenUtils.Margin);

            position = ScreenUtils.UpdateText(Cost, true, position, Base.FormatDisplayText(Base.Cost, false), ScreenUtils.Margin);

            using var amountDisplayText = Base.GetAmountDisplayText().GetEnumerator();
            position = Amounts.Aggregate(position, (currentPosition, amount) => ScreenUtils.UpdateText(amount, amountDisplayText.MoveNext(), currentPosition, amountDisplayText.Current, ScreenUtils.Margin));

            var amountIcon = Base.GetAmountIcon();
            for (var i = 0; i < amountIcon.Count; i++)
            {
                Amounts[i].Icon.sprite = amountIcon[i];
            }

            return position.y + ScreenUtils.Margin;
        }
    }
}
