using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Upgrade.Attribute
{
    [UsedImplicitly]
    public sealed class AttackSpeedUpgrade : AttributeUpgrade
    {
        protected override AttributeValue Attribute => Tower.AttackSpeed;

        public override List<Sprite> GetAmountIcon()
        {
            return new List<Sprite> {Icons.AttackSpeed};
        }
    }
}
