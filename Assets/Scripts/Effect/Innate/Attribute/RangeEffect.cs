using Assets.Scripts.Tower;

namespace Assets.Scripts.Effect.Innate.Attribute
{
    public sealed class RangeEffect : AttributeEffect
    {
        public override string Name => "Range Increase";

        public override AttributeValue GetAttribute(TowerBase tower)
        {
            return tower.Range;
        }
    }
}
