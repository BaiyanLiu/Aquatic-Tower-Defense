namespace Assets.Scripts.Effect
{
    public sealed class DamageEffect : InnateEffect
    {
        public override string Name => "Damage Effect";
        protected override string AmountName => "Damage";
    }
}
