using UnityEngine;

namespace Assets.Scripts.Effect.Area
{
    public sealed class AreaAttackSpeedEffect : AreaTowerEffect
    {
        public override string Name => "Motivation";
        protected override string AmountUnit => "%";
        protected override Sprite AmountIcon => GameState.Instance.IconsByName["Icon_Alarm_Plus"];
        public override Color StatusColor => Colors.Instance.Orange;
    }
}
