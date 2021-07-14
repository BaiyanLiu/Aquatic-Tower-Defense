namespace Assets.Scripts.Effect
{
    public interface IHasEffect 
    {
        public void AddEffect(EffectBase effect);

        public void RemoveEffect(EffectBase effect);
    }
}
