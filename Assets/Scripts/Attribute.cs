using System;

namespace Assets.Scripts
{
    [Serializable]
    public class Attribute<T>
    {
        public T Base;
        public T Gain;

        public T Value { get; set; }
    }
}
