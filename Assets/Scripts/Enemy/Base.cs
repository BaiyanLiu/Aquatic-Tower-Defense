using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class Base : MonoBehaviour
    {
        public float Health { get; private set; } = 100f;
        public float Speed { get; private set; } = 0.05f;
    }
}
