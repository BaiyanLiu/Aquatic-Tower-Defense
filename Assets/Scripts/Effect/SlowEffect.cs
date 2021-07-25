using UnityEngine;

namespace Assets.Scripts.Effect
{
    public sealed class SlowEffect : EffectBase
    {
        public override string Name => "Slow";
        protected override Sprite AmountIcon => GameState.Instance.IconsByName["Icon_Turtle"];
        public override Color StatusColor => Colors.Instance.Blue;
    }
}
