namespace Assets.Scripts.Effect
{
    public class SlowEffect : EffectBase
    {
        public float Amount;
        public float AmountGain;

        public override void LevelUp()
        {
            base.LevelUp();
            Amount += AmountGain;
        }
    }
}
