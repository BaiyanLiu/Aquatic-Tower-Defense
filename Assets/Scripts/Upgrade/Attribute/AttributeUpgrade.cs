namespace Assets.Scripts.Upgrade.Attribute
{
    public abstract class AttributeUpgrade : UpgradeBase
    {
        protected abstract AttributeValue Attribute { get; }

        public sealed override void OnApply()
        {
            Attribute.Value += Amount[Level];
        }
    }
}
