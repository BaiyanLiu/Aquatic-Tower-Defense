using System;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class Base : MonoBehaviour
    {
        public float MaxHealth { get; private set; } = 100f;
        public float Health { get; private set; } = 100f;
        public float Speed { get; private set; } = 0.05f;

        private Animator _animator;

        private Transform _healthBar;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _healthBar = transform.Find("Health Bar").Find("Fill");
        }

        public void UpdateHealth(float delta)
        {
            Health += delta;
            _healthBar.localScale = new Vector2(Math.Max(Health / MaxHealth, 0f), 1f);
            if (Health <= 0)
            {
                _animator.SetBool("Dead", true);
            }
        }
    }
}
