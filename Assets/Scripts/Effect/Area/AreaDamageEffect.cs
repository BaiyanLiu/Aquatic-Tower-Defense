using UnityEngine;

namespace Assets.Scripts.Effect.Area
{
    public sealed class AreaDamageEffect : AreaTowerEffect
    {
        public bool IsIncrease;
        public override string Name => IsIncrease ? "Empower" : "Electric Shock";
        protected override string AmountUnit => "%";
        protected override Sprite AmountIcon => GameState.Instance.IconsByName["Icon_Dagger"];
        public override Color StatusColor => IsIncrease ? Colors.Instance.Orange : Colors.Instance.Purple;
    }
}
