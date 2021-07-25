using UnityEngine;

namespace Assets.Scripts.Effect.Innate
{
    public sealed class SplashEffect : InnateEffect
    {
        public override string Name => "Splash";
        protected override Sprite AmountIcon => GameState.Instance.IconsByName["Icon_Circle_Dot"];
        public override Color StatusColor => Colors.Instance.Red;
    }
}
