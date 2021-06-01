using UnityEngine;

namespace Assets.Scripts.Effect
{
    public class ChainEffect : EffectBase
    {
        public float Damage;
        public float Range;

        public float DamageGain;
        public float RangeGain;

        public override string Name => "Chain";
        public override Color StatusColor => new Color32(255, 242, 0, 255);
        public override bool IsInnate => true;

        public override void LevelUp()
        {
            base.LevelUp();
            Damage += DamageGain;
            Range += RangeGain;
        }
    }
}
