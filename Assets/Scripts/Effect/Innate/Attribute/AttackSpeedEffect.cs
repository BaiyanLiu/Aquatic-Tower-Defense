using Assets.Scripts.Tower;

namespace Assets.Scripts.Effect.Innate.Attribute
{
    public sealed class AttackSpeedEffect : AttributeEffect
    {
        public override string Name => "A. Speed Increase";

        public override AttributeValue GetAttribute(TowerBase tower)
        {
            return tower.AttackSpeed;
        }
    }
}
