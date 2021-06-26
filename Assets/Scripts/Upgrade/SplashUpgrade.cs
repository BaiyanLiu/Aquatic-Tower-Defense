using Assets.Scripts.Effect;
using JetBrains.Annotations;

namespace Assets.Scripts.Upgrade
{
    [UsedImplicitly]
    public sealed class SplashUpgrade : EffectUpgrade<SplashEffect>
    {
        public override string Name => "Splash Upgrade";
        protected override string AmountName => "Range";

        protected override void OnLevelUp()
        {
            base.OnLevelUp();
            Effect.Amount.Value = Amount[Level];
        }
    }
}
