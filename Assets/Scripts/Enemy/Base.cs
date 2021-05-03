using System;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class Base : MonoBehaviour
    {
        public event EventHandler<GameObject> OnDie;

        public bool IsFast;
        public bool IsArmored;

        public float Health { get; private set; }
        public float Speed { get; private set; } = 0.05f;

        private float DamageReduction => (100f - _armor) / 100f;

        private Animator _animator;
        private Transform _healthBar;

        private float _maxHealth = 100f;
        private float _armor = 1f;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _healthBar = transform.Find("Health Bar").Find("Fill");

            if (IsFast)
            {
                Speed *= 2f;
                _maxHealth /= 1.5f;
            }
            if (IsArmored)
            {
                Speed /= 1.5f;
                _maxHealth *= 2f;
                _armor *= 2f;
            }
            Health = _maxHealth;
        }

        public void UpdateHealth(float delta)
        {
            Health += delta * DamageReduction;
            _healthBar.localScale = new Vector2(Math.Max(Health / _maxHealth, 0f), 1f);
            if (Health <= 0)
            {
                _animator.SetBool("Dead", true);
                OnDie?.Invoke(this, gameObject);
            }
        }
    }
}
