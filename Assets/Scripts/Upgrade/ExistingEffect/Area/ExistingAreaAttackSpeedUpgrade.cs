using Assets.Scripts.Effect.Area;
using JetBrains.Annotations;

namespace Assets.Scripts.Upgrade.ExistingEffect.Area
{
    [UsedImplicitly]
    public sealed class ExistingAreaAttackSpeedUpgrade : ExistingAreaEffectUpgrade<AreaAttackSpeedEffect>
    {
        protected override string AmountName => "A. Speed";
        protected override string AmountUnit => "%";
    }
}
