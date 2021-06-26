using Assets.Scripts.Effect;
using JetBrains.Annotations;

namespace Assets.Scripts.Upgrade
{
    [UsedImplicitly]
    public sealed class ExperienceUpgrade : EffectUpgrade<ExperienceEffect>
    {
        public override string Name => "Experience Upgrade";
        protected override string AmountName => "Experience";
        protected override string AmountUnit => "%";
    }
}
