using JetBrains.Annotations;

namespace Assets.Scripts.Upgrade.Attribute
{
    [UsedImplicitly]
    public sealed class DamageUpgrade : AttributeUpgrade
    {
        public override string Name => "Damage Upgrade";
        protected override string AmountName => "Damage";
        protected override Attribute<float> Attribute => Tower.Damage;
    }
}
