namespace Assets.Scripts.Effect.Innate
{
    public sealed class ItemChanceEffect : InnateEffect
    {
        public override string Name => "Item Drop Chance";
        protected override string AmountName => "Chance";
        protected override string AmountUnit => "%";
    }
}
