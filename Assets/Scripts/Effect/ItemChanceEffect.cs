namespace Assets.Scripts.Effect
{
    public sealed class ItemChanceEffect : InnateEffect
    {
        public override string Name => "Item Chance Effect";
        protected override string AmountName => "Chance";
        protected override string AmountUnit => "%";
    }
}
