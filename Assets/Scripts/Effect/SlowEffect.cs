namespace Assets.Scripts.Effect
{
    public class SlowEffect : EffectBase
    {
        public float Amount;

        public float DurationGain;
        public float AmountGain;

        public override void LevelUp()
        {
            Duration += DurationGain;
            Amount += AmountGain;
        }
    }
}
