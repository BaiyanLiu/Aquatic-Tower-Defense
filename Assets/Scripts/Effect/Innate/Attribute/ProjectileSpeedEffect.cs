using Assets.Scripts.Tower;

namespace Assets.Scripts.Effect.Innate.Attribute
{
    public sealed class ProjectileSpeedEffect : AttributeEffect
    {
        public override string Name => "P. Speed Increase";

        public override AttributeValue GetAttribute(TowerBase tower)
        {
            return tower.ProjectileSpeed;
        }
    }
}
