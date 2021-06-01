using UnityEngine;

namespace Assets.Scripts.Effect
{
    public class SplashEffect : EffectBase
    {
        public float Range;
        public float RangeGain;

        public override string Name => "Splash";
        public override Color StatusColor => new Color32(255, 0, 0, 255);
        public override bool IsInnate => true;

        public override void LevelUp()
        {
            base.LevelUp();
            Range += RangeGain;
        }
    }
}
