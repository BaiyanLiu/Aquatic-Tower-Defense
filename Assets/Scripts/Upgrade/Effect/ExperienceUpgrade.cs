using Assets.Scripts.Effect.Innate;
using JetBrains.Annotations;

namespace Assets.Scripts.Upgrade.Effect
{
    [UsedImplicitly]
    public sealed class ExperienceUpgrade : EffectUpgrade<ExperienceEffect>
    {
        protected override string AmountUnit => "%";
    }
}
