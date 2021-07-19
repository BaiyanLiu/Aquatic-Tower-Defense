using Assets.Scripts.Tower;

namespace Assets.Scripts.Effect.Innate.Attribute
{
    public abstract class AttributeEffect : InnateEffect
    {
        public bool IsMultiply = false;

        public abstract AttributeValue GetAttribute(TowerBase tower);
    }
}
