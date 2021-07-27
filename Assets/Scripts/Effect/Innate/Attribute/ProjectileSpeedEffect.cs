using Assets.Scripts.Tower;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Effect.Innate.Attribute
{
    [UsedImplicitly]
    public sealed class ProjectileSpeedEffect : AttributeEffect
    {
        public override string Name => "P. Speed Increase";
        protected override Sprite AmountIcon => Icons.ProjectileSpeed;

        protected override AttributeValue GetAttribute(TowerBase tower)
        {
            return tower.ProjectileSpeed;
        }
    }
}
