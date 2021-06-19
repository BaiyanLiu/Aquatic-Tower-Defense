using JetBrains.Annotations;

namespace Assets.Scripts.Upgrade
{
    [UsedImplicitly]
    public class DamageUpgrade : UpgradeBase
    {
        public override string Name => "Damage Upgrade";
        protected override string AmountName => "Damage";
    }
}
