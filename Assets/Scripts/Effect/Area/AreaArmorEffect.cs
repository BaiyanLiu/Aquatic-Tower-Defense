using UnityEngine;

namespace Assets.Scripts.Effect.Area
{
    public sealed class AreaArmorEffect : EnemyAreaEffect
    {
        public bool IsIncrease;
        public override string Name => IsIncrease ? "Armor Chant" : "Tremble";
        protected override string AmountName => "Armor";
        public override Color StatusColor => IsIncrease ? Colors.Instance.Yellow : Colors.Instance.Purple;
    }
}
