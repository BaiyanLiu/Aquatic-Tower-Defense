using Assets.Scripts.Effect;
using JetBrains.Annotations;

namespace Assets.Scripts.Upgrade
{
    [UsedImplicitly]
    public sealed class ItemChanceUpgrade : EffectUpgrade<ItemChanceEffect>
    {
        public override string Name => "Item Chance Upgrade";
        protected override string AmountName => "Chance";
        protected override string AmountUnit => "%";
    }
}
