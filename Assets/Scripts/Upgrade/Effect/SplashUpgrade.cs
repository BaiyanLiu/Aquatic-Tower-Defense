using Assets.Scripts.Effect.Innate;
using JetBrains.Annotations;

namespace Assets.Scripts.Upgrade.Effect
{
    [UsedImplicitly]
    public sealed class SplashUpgrade : EffectUpgrade<SplashEffect>
    {
        public override string Name => "Splash Upgrade";
        protected override string AmountName => "Range";
    }
}
