using Assets.Scripts.Effect;
using JetBrains.Annotations;

namespace Assets.Scripts.Upgrade
{
    [UsedImplicitly]
    public sealed class ExistingSplashUpgrade : ExistingEffectUpgrade<SplashEffect>
    {
        public override string Name => "Splash Upgrade";
        protected override string AmountName => "Splash";

        protected override void OnLevelUp()
        {
            var delta = Amount[Level];
            if (Level > 0)
            {
                delta -= Amount[Level - 1];
            }
            Effect.Amount.Base += delta;
            Effect.Amount.Value += delta;
        }
    }
}
