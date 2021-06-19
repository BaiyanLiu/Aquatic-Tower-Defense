using UnityEngine;

namespace Assets.Scripts.Effect
{
    public sealed class SplashEffect : InnateEffect
    {
        public override string Name => "Splash Effect";
        protected override string AmountName => "Range";
        public override Color StatusColor => new Color32(255, 0, 0, 255);
    }
}
