using Assets.Scripts.Effect.Innate;
using JetBrains.Annotations;

namespace Assets.Scripts.Upgrade.ExistingEffect
{
    [UsedImplicitly]
    public sealed class ExistingSplashUpgrade : ExistingEffectUpgrade<SplashEffect>
    {
        public override string Name => "Splash Upgrade";
        protected override string AmountName => "Range";

        protected override void OnLevelUp()
        {
            var delta = GetDelta(Amount);
            Effect.Amount.Base += delta;
            Effect.Amount.Value += delta;
        }
    }
}
