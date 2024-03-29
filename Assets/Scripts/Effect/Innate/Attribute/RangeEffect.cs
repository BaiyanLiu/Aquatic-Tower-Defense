using Assets.Scripts.Tower;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Effect.Innate.Attribute
{
    [UsedImplicitly]
    public sealed class RangeEffect : AttributeEffect
    {
        public override string Name => "Range Increase";
        protected override Sprite AmountIcon => Icons.Range;

        protected override AttributeValue GetAttribute(TowerBase tower)
        {
            return tower.Range;
        }
    }
}
