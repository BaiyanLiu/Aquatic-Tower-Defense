using System;
using System.Linq;
using Assets.Scripts.Effect;
using Assets.Scripts.Persistence;
using Assets.Scripts.Tower;
using UnityEngine;

namespace Assets.Scripts.Item
{
    public sealed class ItemBase : MonoBehaviour, ICloneable
    {
        public string Name;
        public int Level { get; private set; }
        public EffectBase[] Effects { get; set; }
        public TowerBase Tower { get; private set; }

        public void UpdateLevel(int level)
        {
            Level = level;
            foreach (var effect in Effects)
            {
                effect.UpdateLevel(level);
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
            var effects = (from effect in GetComponents<EffectBase>() select (EffectBase) effect.Clone()).ToArray();
            foreach (var effect in effects)
            {
                effect.Item = this;
                effect.IncludeGain = false;
            }
            ((ItemBase) clone).Effects = effects;
            return clone;
        }

        public ItemSnapshot ToSnapshot()
        {
            return new ItemSnapshot
            {
                Name = Name,
                Level = Level
            };
        }

        public static ItemBase FromSnapshot(ItemSnapshot snapshot)
        {
            var item = (ItemBase) GameState.Instance.ItemsByName[snapshot.Name].Clone();
            item.UpdateLevel(snapshot.Level);
            return item;
        }
    }
}
