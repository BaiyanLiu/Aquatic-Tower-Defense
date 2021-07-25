using UnityEngine;

namespace Assets.Scripts.Effect.Area
{
    public sealed class AreaSlowEffect : EnemyAreaEffect
    {
        public override string Name => "Intimidation";
        protected override Sprite AmountIcon => GameState.Instance.IconsByName["Icon_Turtle"];
        public override Color StatusColor => Colors.Instance.Blue;
    }
}
