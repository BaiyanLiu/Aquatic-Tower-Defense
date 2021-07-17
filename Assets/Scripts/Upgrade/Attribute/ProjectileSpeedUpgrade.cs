using JetBrains.Annotations;

namespace Assets.Scripts.Upgrade.Attribute
{
    [UsedImplicitly]
    public sealed class ProjectileSpeedUpgrade : AttributeUpgrade
    {
        protected override string AmountName => "P. Speed";
        protected override AttributeValue Attribute => Tower.ProjectileSpeed;
    }
}
