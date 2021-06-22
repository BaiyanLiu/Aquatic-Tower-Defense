namespace Assets.Scripts.Upgrade
{
    public abstract class AttributeUpgrade : UpgradeBase
    {
        protected abstract Attribute<float> Attribute { get; }

        public sealed override void OnApply()
        {
            Attribute.Value += Amount[Level];
        }
    }
}
