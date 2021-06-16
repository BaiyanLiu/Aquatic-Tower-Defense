using System;
using System.Linq;
using Assets.Scripts.Effect;
using Assets.Scripts.Tower;
using UnityEngine;

namespace Assets.Scripts.Item
{
    public class ItemBase : MonoBehaviour, ICloneable
    {
        public string Name;
        public EffectBase[] Effects { get; set; }
        public TowerBase Tower { get; private set; }

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

        public void UpdateTower(TowerBase tower)
        {
            Tower = tower;
            foreach (var effect in Effects)
            {
                effect.Tower = tower;
            }
        }

        public object Clone()
        {
            var clone = MemberwiseClone();
            var effects = (from effect in (EffectBase[]) GetComponents<EffectBase>().Clone() select (EffectBase) effect.Clone()).ToArray();
            foreach (var effect in effects)
            {
                effect.Item = this;
            }
            ((ItemBase) clone).Effects = effects;
            return clone;
        }
    }
}
