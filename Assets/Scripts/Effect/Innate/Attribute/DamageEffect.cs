using Assets.Scripts.Tower;
using JetBrains.Annotations;

namespace Assets.Scripts.Effect.Innate.Attribute
{
    [UsedImplicitly]
    public sealed class DamageEffect : AttributeEffect
    {
        public override string Name => "Damage Increase";

        protected override AttributeValue GetAttribute(TowerBase tower)
        {
            return tower.Damage;
        }
    }
}
