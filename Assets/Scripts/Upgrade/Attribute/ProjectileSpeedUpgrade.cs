using JetBrains.Annotations;

namespace Assets.Scripts.Upgrade.Attribute
{
    [UsedImplicitly]
    public sealed class ProjectileSpeedUpgrade : AttributeUpgrade
    {
        public override string Name => "Projectile Speed Upgrade";
        protected override string AmountName => "P. Speed";
        protected override Attribute<float> Attribute => Tower.ProjectileSpeed;
    }
}
