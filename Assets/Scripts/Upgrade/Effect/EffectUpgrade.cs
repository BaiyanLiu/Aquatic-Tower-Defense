using Assets.Scripts.Effect;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Upgrade.Effect
{
    public abstract class EffectUpgrade<T> : UpgradeBase where T : EffectBase
    {
        public sealed override Color NameColor => Effect.StatusColor;

        protected T Effect;

        [UsedImplicitly]
        private void Start()
        {
            Effect = gameObject.AddComponent<T>();
            Effect.Tower = Tower;
            Effect.IncludeGain = false;
        }

        protected override void OnLevelUp()
        {
            if (Level == 0)
            {
                Tower.Effects.Add(Effect);
                Tower.AllEffects.Add(Effect);
            }
            Effect.Amount.Value = Amount[Level];
        }
    }
}
