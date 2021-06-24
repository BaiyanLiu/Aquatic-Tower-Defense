using Assets.Scripts.Effect;

namespace Assets.Scripts.Upgrade
{
    public abstract class ExistingEffectUpgrade<T> : UpgradeBase where T : EffectBase
    {
        protected T Effect;

        protected sealed override void OnStart()
        {
            Effect = gameObject.GetComponent<T>();
        }
    }
}
