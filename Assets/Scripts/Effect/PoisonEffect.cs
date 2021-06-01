using UnityEngine;

namespace Assets.Scripts.Effect
{
    public class PoisonEffect : EffectBase
    {
        public float Damage;
        public float DamageGain;

        public override string Name => "Poison";
        public override Color StatusColor => new Color32(34, 177, 76, 255);
        public override bool IsInnate => false;

        public override void LevelUp()
        {
            base.LevelUp();
            Damage += DamageGain;
        }
    }
}
