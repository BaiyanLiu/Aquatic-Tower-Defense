using UnityEngine;

namespace Assets.Scripts.Effect
{
    public class SplashEffect : InnateEffect
    {
        public override string Name => "Splash Effect";
        public override string AmountName => "Range";
        public override Color StatusColor => new Color32(255, 0, 0, 255);
    }
}
