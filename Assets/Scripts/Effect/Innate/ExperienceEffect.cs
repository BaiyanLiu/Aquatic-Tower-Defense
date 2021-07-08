namespace Assets.Scripts.Effect.Innate
{
    public sealed class ExperienceEffect : InnateEffect
    {
        public override string Name => "Experience Bonus";
        protected override string AmountName => "Experience";
        protected override string AmountUnit => "%";
    }
}
