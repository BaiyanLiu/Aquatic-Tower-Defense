using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Upgrade.Attribute
{
    [UsedImplicitly]
    public sealed class RangeUpgrade : AttributeUpgrade
    {
        protected override AttributeValue Attribute => Tower.Range;

        public override List<Sprite> GetAmountIcon()
        {
            return new List<Sprite> {Icons.Range};
        }
    }
}
