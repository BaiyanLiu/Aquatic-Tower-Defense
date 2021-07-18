using Assets.Scripts.Effect.Area;
using JetBrains.Annotations;

namespace Assets.Scripts.Upgrade.Effect.Area
{
    [UsedImplicitly]
    public sealed class AreaDamageUpgrade : AreaEffectUpgrade<AreaDamageEffect>
    {
        protected override string AmountName => "Damage";
        protected override string AmountUnit => "%";

        protected override void OnStart()
        {
            base.OnStart();
            Effect.IsIncrease = true;
        }
    }
}
