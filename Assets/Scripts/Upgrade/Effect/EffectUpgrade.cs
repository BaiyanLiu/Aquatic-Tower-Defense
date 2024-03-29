using System.Collections.Generic;
using Assets.Scripts.Effect;
using UnityEngine;

namespace Assets.Scripts.Upgrade.Effect
{
    public abstract class EffectUpgrade<T> : UpgradeBase where T : EffectBase
    {
        public sealed override Color NameColor => Effect.StatusColor;

        protected T Effect;

        protected override void OnStart()
        {
            Effect = gameObject.AddComponent<T>();
            Effect.Source = Tower;
            Effect.IncludeGain = false;
            Effect.IsLoading = IsLoading;
        }

        protected override void OnLevelUp()
        {
            if (Level == 0 || IsLoading)
            {
                Tower.Effects.Add(Effect);
                Tower.AllEffects.Add(Effect);
            }

            if (IsLoading)
            {
                InitForLoading();
            }
            Effect.Amount.Value = Amount[Level];
        }

        protected virtual void InitForLoading()
        {
            Effect.Duration = new AttributeValue();
            Effect.Frequency = new AttributeValue();
            Effect.Amount = new AttributeValue();
        }

        public override List<Sprite> GetAmountIcon()
        {
            return Effect.GetAmountIcon();
        }
    }
}
