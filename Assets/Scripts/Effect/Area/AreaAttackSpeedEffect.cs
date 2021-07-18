using UnityEngine;

namespace Assets.Scripts.Effect.Area
{
    public sealed class AreaAttackSpeedEffect : AreaTowerEffect
    {
        public override string Name => "Motivation";
        protected override string AmountName => "A. Speed";
        protected override string AmountUnit => "%";
        public override Color StatusColor => Colors.Instance.Orange;
    }
}
