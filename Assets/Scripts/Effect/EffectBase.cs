using System;
using UnityEngine;

namespace Assets.Scripts.Effect
{
    public abstract class EffectBase : MonoBehaviour, ICloneable
    {
        public float Duration;

        public abstract void LevelUp();

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
