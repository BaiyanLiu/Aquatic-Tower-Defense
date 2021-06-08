using System;
using Assets.Scripts.Effect;
using UnityEngine;

namespace Assets.Scripts.Item
{
    public class ItemBase : MonoBehaviour, ICloneable
    {
        public string Name;
        public EffectBase[] Effects { get; private set; }

        public int Level
        {
            set
            {
                foreach (var effect in Effects)
                {
                    for (var i = 1; i < value; i++)
                    {
                        effect.LevelUp();
                    }
                }
            }
        }

        public object Clone()
        {
            var clone = MemberwiseClone();
            ((ItemBase) clone).Effects = GetComponents<EffectBase>();
            return clone;
        }
    }
}
