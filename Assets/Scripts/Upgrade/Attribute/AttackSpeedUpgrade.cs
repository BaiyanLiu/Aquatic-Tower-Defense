using JetBrains.Annotations;

namespace Assets.Scripts.Upgrade.Attribute
{
    [UsedImplicitly]
    public sealed class AttackSpeedUpgrade : AttributeUpgrade
    {
        protected override string AmountName => "A. Speed";
        protected override AttributeValue Attribute => Tower.AttackSpeed;
    }
}
