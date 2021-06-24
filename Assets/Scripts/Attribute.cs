using System;

namespace Assets.Scripts
{
    [Serializable]
    public class Attribute<T> : ICloneable
    {
        public T Base;
        public T Gain;

        public T Value { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
