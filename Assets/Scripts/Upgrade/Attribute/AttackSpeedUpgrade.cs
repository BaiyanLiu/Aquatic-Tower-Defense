using JetBrains.Annotations;

namespace Assets.Scripts.Upgrade.Attribute
{
    [UsedImplicitly]
    public sealed class AttackSpeedUpgrade : AttributeUpgrade
    {
        public override string Name => "Attack Speed Upgrade";
        protected override string AmountName => "A. Speed";
        protected override Attribute<float> Attribute => Tower.AttackSpeed;
    }
}
