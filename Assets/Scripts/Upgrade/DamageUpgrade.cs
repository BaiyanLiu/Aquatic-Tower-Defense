using JetBrains.Annotations;

namespace Assets.Scripts.Upgrade
{
    [UsedImplicitly]
    public sealed class DamageUpgrade : UpgradeBase
    {
        public override string Name => "Damage Upgrade";
        protected override string AmountName => "Damage";

        public override void OnApply()
        {
            Tower.Damage.Value += Amount[Level];
        }
    }
}
