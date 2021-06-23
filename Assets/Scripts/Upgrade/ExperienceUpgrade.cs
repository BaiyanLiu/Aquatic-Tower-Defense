using Assets.Scripts.Effect;
using JetBrains.Annotations;

namespace Assets.Scripts.Upgrade
{
    [UsedImplicitly]
    public sealed class ExperienceUpgrade : UpgradeBase
    {
        public override string Name => "Experience Upgrade";
        protected override string AmountName => "Experience";
        protected override string AmountUnit => "%";

        private EffectBase _effect;

        protected override void OnStart()
        {
            _effect = gameObject.AddComponent<ExperienceEffect>();
            _effect.IncludeGain = false;
        }

        protected override void OnLevelUp()
        {
            if (Level == 0)
            {
                Tower.Effects.Add(_effect);
                Tower.AllEffects.Add(_effect);
            }
            _effect.Amount.Value = Amount[Level];
        }
    }
}
