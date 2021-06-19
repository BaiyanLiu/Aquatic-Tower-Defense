using System.Linq;
using Assets.Scripts.Upgrade;
using UnityEngine.UI;

namespace Assets.Scripts.Screens
{
    public class UpgradeDetails : DetailsScreen<UpgradeBase>
    {
        public Text CostText;
        public Text[] AmountTexts;

        protected override float OnUpdate(float height)
        {
            NameText.text = Base.Name;
            height += NameText.rectTransform.rect.height + 5f;
            height = ScreenUtils.UpdateText(CostText, true, 5f, height, Base.FormatDisplayText("Cost", Base.Cost));

            using var amountDisplayText = Base.GetAmountDisplayText().GetEnumerator();
            height = AmountTexts.Aggregate(height, (currentHeight, amountText) => ScreenUtils.UpdateText(amountText, amountDisplayText.MoveNext(), 5f, currentHeight, amountDisplayText.Current));

            return height + 5f;
        }
    }
}
