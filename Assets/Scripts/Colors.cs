using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts
{
    public class Colors : MonoBehaviour
    {
        public static Colors Instance { get; private set; }

        public Color Green;
        public Color Yellow;
        public Color Orange;
        public Color Red;
        public Color Blue;

        [UsedImplicitly]
        private void Start()
        {
            Instance = this;
        }
    }
}
