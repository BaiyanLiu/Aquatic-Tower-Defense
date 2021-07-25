using UnityEngine;

namespace Assets.Scripts.Effect.Area
{
    public sealed class AreaArmorEffect : EnemyAreaEffect
    {
        public bool IsIncrease;
        public override string Name => IsIncrease ? "Armor Chant" : "Tremble";
        protected override Sprite AmountIcon => GameState.Instance.IconsByName[IsIncrease ? "Icon_Shield_Plus" : "Icon_Shield_Minus"];
        public override Color StatusColor => IsIncrease ? Colors.Instance.Yellow : Colors.Instance.Purple;
    }
}
