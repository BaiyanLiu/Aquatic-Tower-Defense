using UnityEngine;

namespace Assets.Scripts.Effect.Innate
{
    public sealed class SplashEffect : InnateEffect
    {
        public override string Name => "Splash";
        protected override string AmountName => "Range";
        public override Color StatusColor => Colors.Instance.Red;
    }
}
