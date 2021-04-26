using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class Base : MonoBehaviour
    {
        public float Damage { get; private set; } = 30f;
        public float Range { get; private set; } = 5f;
        public float AttackSpeed { get; private set; } = 1f;
        public float ProjectileSpeed { get; private set; } = 0.5f;

        private void Start()
        {
            GetComponent<CircleCollider2D>().radius = Range;
        }
    }
}
