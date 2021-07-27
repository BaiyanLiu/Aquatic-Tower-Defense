using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Upgrade
{
    [UsedImplicitly]
    public sealed class SellCostUpgrade : UpgradeBase
    {
        protected override string AmountUnit => "%";

        public override void OnApply()
        {
            Tower.SellCost.Value = Tower.SellCost.Value * Amount[Level] / 100f;
        }

        public override List<Sprite> GetAmountIcon()
        {
            return new List<Sprite> { GameState.Instance.IconsByName["Icon_Tags"] };
        }
    }
}
