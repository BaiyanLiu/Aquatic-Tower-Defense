namespace Assets.Scripts.Effect
{
    public sealed class ExperienceEffect : InnateEffect
    {
        public override string Name => "Experience Effect";
        protected override string AmountName => "Experience";
        protected override string AmountUnit => "%";
    }
}
