using Assets.Scripts.Tower;

namespace Assets.Scripts.Effect.Innate.Attribute
{
    public sealed class DamageEffect : AttributeEffect
    {
        public override string Name => "Damage Increase";

        public override AttributeValue GetAttribute(TowerBase tower)
        {
            return tower.Damage;
        }
    }
}
