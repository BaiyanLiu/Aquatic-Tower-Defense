using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Upgrade.Attribute
{
    [UsedImplicitly]
    public sealed class DamageUpgrade : AttributeUpgrade
    {
        protected override AttributeValue Attribute => Tower.Damage;

        public override List<Sprite> GetAmountIcon()
        {
            return new List<Sprite> {Icons.Damage};
        }
    }
}
