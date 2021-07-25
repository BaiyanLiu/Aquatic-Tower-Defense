using UnityEngine;

namespace Assets.Scripts.Effect
{
    public sealed class SlowEffect : EffectBase
    {
        public override string Name => "Slow";
        protected override Sprite AmountIcon => GameState.Instance.IconsByName["Icon_Rabbit_Running"];
        public override Color StatusColor => Colors.Instance.Blue;
    }
}
