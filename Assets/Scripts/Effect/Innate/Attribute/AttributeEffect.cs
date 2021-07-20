using Assets.Scripts.Tower;

namespace Assets.Scripts.Effect.Innate.Attribute
{
    public abstract class AttributeEffect : InnateEffect
    {
        public bool IsMultiply = false;

        protected override string AmountUnit => IsMultiply ? "%" : "";

        protected abstract AttributeValue GetAttribute(TowerBase tower);

        public void Apply()
        {
            if (Source is TowerBase tower)
            {
                var attribute = GetAttribute(tower);
                if (IsMultiply)
                {
                    OnApplyMultiply(attribute);
                }
                else
                {
                    OnApplyAdd(attribute);
                }
            }
        }

        protected virtual void OnApplyMultiply(AttributeValue attribute)
        {
            attribute.Value *= Amount.Value / 100f;
        }

        protected virtual void OnApplyAdd(AttributeValue attribute)
        {
            attribute.Value += Amount.Value;
        }
    }
}
