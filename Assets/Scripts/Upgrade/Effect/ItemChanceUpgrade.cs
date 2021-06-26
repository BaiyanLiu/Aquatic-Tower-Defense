using Assets.Scripts.Effect.Innate;
using JetBrains.Annotations;

namespace Assets.Scripts.Upgrade.Effect
{
    [UsedImplicitly]
    public sealed class ItemChanceUpgrade : EffectUpgrade<ItemChanceEffect>
    {
        public override string Name => "Item Chance Upgrade";
        protected override string AmountName => "Chance";
        protected override string AmountUnit => "%";
    }
}
