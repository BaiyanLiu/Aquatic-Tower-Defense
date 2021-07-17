using Assets.Scripts.Tower;
using UnityEngine;

namespace Assets.Scripts.Effect.Area
{
    public sealed class AreaAttackSpeedEffect : AreaEffect<TowerBase>
    {
        public override string Name => "Motivation";
        protected override string AmountName => "A. Speed";
        protected override string AmountUnit => "%";
        public override Color StatusColor => Colors.Instance.Orange;
    }
}
