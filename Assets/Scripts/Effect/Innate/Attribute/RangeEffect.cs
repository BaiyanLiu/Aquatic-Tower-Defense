using Assets.Scripts.Tower;
using JetBrains.Annotations;

namespace Assets.Scripts.Effect.Innate.Attribute
{
    [UsedImplicitly]
    public sealed class RangeEffect : AttributeEffect
    {
        public override string Name => "Range Increase";

        protected override AttributeValue GetAttribute(TowerBase tower)
        {
            return tower.Range;
        }
    }
}
