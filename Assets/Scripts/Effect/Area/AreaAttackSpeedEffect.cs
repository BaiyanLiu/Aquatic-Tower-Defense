using UnityEngine;

namespace Assets.Scripts.Effect.Area
{
    public sealed class AreaAttackSpeedEffect : AreaTowerEffect
    {
        public override string Name => "Motivation";
        protected override string AmountUnit => "%";
        protected override Sprite AmountIcon => Icons.AttackSpeed;
        public override Color StatusColor => Colors.Instance.Orange;
    }
}
