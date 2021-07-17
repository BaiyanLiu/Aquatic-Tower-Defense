using Assets.Scripts.Enemy;
using UnityEngine;

namespace Assets.Scripts.Effect.Area
{
    public sealed class AreaArmorEffect : AreaEffect<EnemyBase>
    {
        public override string Name => Amount.Base > 0f ? "Armor Chant" : "Tremble";
        protected override string AmountName => "Armor";
        public override Color StatusColor => Amount.Base > 0f ? Colors.Instance.Yellow : Colors.Instance.Purple;
    }
}
