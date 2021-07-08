using UnityEngine;

namespace Assets.Scripts.Effect.Innate
{
    public sealed class SplashEffect : InnateEffect
    {
        public override string Name => "Splash";
        protected override string AmountName => "Range";
        public override Color StatusColor => new Color32(237, 28, 36, 255);
    }
}
