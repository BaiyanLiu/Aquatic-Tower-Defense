using Assets.Scripts.Effect;

namespace Assets.Scripts.Upgrade
{
    public abstract class EffectUpgrade<T> : UpgradeBase where T : EffectBase
    {
        protected T Effect;

        protected sealed override void OnStart()
        {
            Effect = gameObject.AddComponent<T>();
            Effect.IncludeGain = false;
        }

        protected override void OnLevelUp()
        {
            if (Level == 0)
            {
                Tower.Effects.Add(Effect);
                Tower.AllEffects.Add(Effect);
            }
        }
    }
}
