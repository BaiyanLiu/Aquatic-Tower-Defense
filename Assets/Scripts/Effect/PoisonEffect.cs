namespace Assets.Scripts.Effect
{
    public class PoisonEffect : EffectBase
    {
        public float Damage;
        public float DamageGain;

        public override void LevelUp()
        {
            base.LevelUp();
            Damage += DamageGain;
        }
    }
}
