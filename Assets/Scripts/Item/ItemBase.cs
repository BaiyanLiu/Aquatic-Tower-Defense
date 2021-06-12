using System;
using System.Linq;
using Assets.Scripts.Effect;
using UnityEngine;

namespace Assets.Scripts.Item
{
    public class ItemBase : MonoBehaviour, ICloneable
    {
        public string Name;
        public EffectBase[] Effects { get; set; }

        public int Level
        {
            set
            {
                foreach (var effect in Effects)
                {
                    effect.UpdateLevel(value);
                }
            }
        }

        public object Clone()
        {
            var clone = MemberwiseClone();
            ((ItemBase) clone).Effects = (from effect in (EffectBase[]) GetComponents<EffectBase>().Clone() select (EffectBase) effect.Clone()).ToArray();
            return clone;
        }
    }
}
