using System;

namespace Assets.Scripts
{
    [Serializable]
    public class AttributeValue : ICloneable
    {
        public float Base;
        public float Gain;

        public float Value { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
