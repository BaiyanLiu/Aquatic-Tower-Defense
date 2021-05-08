using System;
using Assets.Scripts.Tower;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class Base : MonoBehaviour
    {
        public event EventHandler<GameObject> OnDie;

        public float MaxHealthBase;
        public float ArmorBase;

        public float MaxHealthGain;
        public float ArmorGain;

        public float Speed;
        public ArmorType ArmorType;
        public int Experience;
        
        public float Health { get; private set; }
        public int Level
        {
            set
            {
                _maxHealth = MaxHealthBase + MaxHealthGain * value;
                _armor = ArmorBase + ArmorGain * value;
                Health = _maxHealth;
            }
        }

        private float DamageReduction => (100f - _armor) / 100f;

        private Animator _animator;
        private Transform _healthBar;

        private float _maxHealth;
        private float _armor;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _healthBar = transform.Find("Health Bar").Find("Fill");
        }

        public bool UpdateHealth(float delta, DamageType damageType)
        {
            Health += delta * DamageReduction * DamageTypeReduction(damageType);
            _healthBar.localScale = new Vector2(Math.Max(Health / _maxHealth, 0f), 1f);
            if (Health <= 0)
            {
                _animator.SetBool("Dead", true);
                OnDie?.Invoke(this, gameObject);
                return true;
            }
            return false;
        }

        private float DamageTypeReduction(DamageType damageType)
        {
            return ArmorType switch
            {
                ArmorType.Light => damageType switch
                {
                    DamageType.Light => 1f,
                    DamageType.Normal => 1.1f,
                    DamageType.Heavy => 1.2f,
                    _ => 1f
                },
                ArmorType.Normal => damageType switch
                {
                    DamageType.Light => 0.8f,
                    DamageType.Normal => 1f,
                    DamageType.Heavy => 1.1f,
                    _ => 1f
                },
                ArmorType.Heavy => damageType switch
                {
                    DamageType.Light => 0.6f,
                    DamageType.Normal => 0.8f,
                    DamageType.Heavy => 1f,
                    _ => 1f
                },
                _ => 1f
            };
        }
    }
}
