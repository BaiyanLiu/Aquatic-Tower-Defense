using Assets.Scripts.Effect.Innate;
using JetBrains.Annotations;

namespace Assets.Scripts.Upgrade.Effect
{
    [UsedImplicitly]
    public sealed class ExperienceUpgrade : EffectUpgrade<ExperienceEffect>
    {
        public override string Name => "Experience Upgrade";
        protected override string AmountName => "Experience";
        protected override string AmountUnit => "%";
    }
}
