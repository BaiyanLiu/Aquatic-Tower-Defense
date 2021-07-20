namespace Assets.Scripts.Effect.Innate
{
    public sealed class GoldEffect : InnateEffect
    {
        public override string Name => "Gold Bonus";
        protected override string AmountName => "Gold";
        protected override string AmountUnit => "%";
    }
}
