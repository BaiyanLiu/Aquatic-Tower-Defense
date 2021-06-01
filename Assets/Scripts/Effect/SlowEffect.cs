using UnityEngine;

namespace Assets.Scripts.Effect
{
    public class SlowEffect : EffectBase
    {
        public float Amount;
        public float AmountGain;

        public override string Name => "Slow";
        public override Color StatusColor => new Color32(0, 162, 232, 255);
        public override bool IsInnate => false;

        public override void LevelUp()
        {
            base.LevelUp();
            Amount += AmountGain;
        }
    }
}
