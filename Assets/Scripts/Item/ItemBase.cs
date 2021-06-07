using Assets.Scripts.Effect;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Item
{
    public class ItemBase : MonoBehaviour
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

        [UsedImplicitly]
        private void Start()
        {
            Effects = GetComponents<EffectBase>();
        }
    }
}
