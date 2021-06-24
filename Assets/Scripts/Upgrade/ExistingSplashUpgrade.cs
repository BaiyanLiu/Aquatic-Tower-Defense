using Assets.Scripts.Effect;
using JetBrains.Annotations;

namespace Assets.Scripts.Upgrade
{
    [UsedImplicitly]
    public sealed class ExistingSplashUpgrade : UpgradeBase
    {
        public override string Name => "Splash Upgrade";
        protected override string AmountName => "Splash";

        private EffectBase _effect;

        protected override void OnStart()
        {
            _effect = gameObject.GetComponent<SplashEffect>();
        }

        protected override void OnLevelUp()
        {
            var delta = Amount[Level];
            if (Level > 0)
            {
                delta -= Amount[Level - 1];
            }
            _effect.Amount.Base += delta;
            _effect.Amount.Value += delta;
        }
    }
}
