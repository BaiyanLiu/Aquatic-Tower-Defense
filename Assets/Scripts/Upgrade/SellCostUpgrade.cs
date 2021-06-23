using JetBrains.Annotations;

namespace Assets.Scripts.Upgrade
{
    [UsedImplicitly]
    public sealed class SellCostUpgrade : UpgradeBase
    {
        public override string Name => "Sell Cost Upgrade";
        protected override string AmountName => "Sell Cost";
        protected override string AmountUnit => "%";

        public override void OnApply()
        {
            Tower.SellCost.Value = Tower.SellCost.Value * Amount[Level] / 100f;
        }
    }
}
