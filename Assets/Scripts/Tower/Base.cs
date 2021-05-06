using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class Base : MonoBehaviour
    {
        public float Damage;
        public float Range;
        public float AttackSpeed;
        public float ProjectileSpeed;
        public float Splash;
        public DamageType DamageType;

        private void Start()
        {
            GetComponent<CircleCollider2D>().radius = Range;
        }
    }
}
