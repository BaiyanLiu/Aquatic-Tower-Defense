using Assets.Scripts.Effect;
using UnityEngine;

namespace Assets.Scripts.Upgrade.ExistingEffect
{
    public abstract class ExistingEffectUpgrade<T> : UpgradeBase where T : EffectBase
    {
        public sealed override Color NameColor => Effect.StatusColor;

        protected T Effect;

        protected override void OnStart()
        {
            Effect = GetComponent<T>();
        }

        protected float GetDelta(float[] attribute)
        {
            var delta = attribute[Level];
            if (Level > 0 && !IsLoading)
            {
                delta -= attribute[Level - 1];
            }
            return delta;
        }
    }
}
