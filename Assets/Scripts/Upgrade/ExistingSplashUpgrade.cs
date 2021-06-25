using Assets.Scripts.Effect;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Upgrade
{
    [UsedImplicitly]
    public sealed class ExistingSplashUpgrade : ExistingEffectUpgrade<SplashEffect>
    {
        public override string Name => "Splash Upgrade";
        protected override string AmountName => "Splash";
        public override Color TitleColor => new Color32(237, 28, 36, 255);

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
