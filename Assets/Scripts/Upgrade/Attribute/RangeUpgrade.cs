using JetBrains.Annotations;

namespace Assets.Scripts.Upgrade.Attribute
{
    [UsedImplicitly]
    public sealed class RangeUpgrade : AttributeUpgrade
    {
        protected override string AmountName => "Range";
        protected override AttributeValue Attribute => Tower.Range;
    }
}
