using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts
{
    public class Colors : MonoBehaviour
    {
        public static Colors Instance { get; private set; }

        public Color Green;
        public Color Yellow;
        public Color Red;

        [UsedImplicitly]
        private void Start()
        {
            Instance = this;
        }
    }
}
