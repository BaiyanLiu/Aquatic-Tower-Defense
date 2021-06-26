using Assets.Scripts.Effect;
using UnityEngine;

namespace Assets.Scripts.Upgrade
{
    public abstract class ExistingEffectUpgrade<T> : UpgradeBase where T : EffectBase
    {
        public sealed override Color NameColor => Effect.StatusColor;

        protected T Effect;

        protected sealed override void OnStart()
        {
            Effect = gameObject.GetComponent<T>();
        }
    }
}
