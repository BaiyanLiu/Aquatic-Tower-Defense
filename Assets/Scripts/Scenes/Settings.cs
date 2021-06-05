using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Scenes
{
    public class Settings : MonoBehaviour
    {
        public const string Health = "Health";

        public Slider HealthSlider;
        public Text HealthValue;

        private int _health;

        [UsedImplicitly]
        private void Start()
        {
            if (!PlayerPrefs.HasKey(Health))
            {
                PlayerPrefs.SetInt(Health, 100);
            }
            HealthSlider.value = _health = PlayerPrefs.GetInt(Health);
        }

        [UsedImplicitly]
        public void OnHealthChanged(float value)
        {
            _health = Convert.ToInt16(value);
            HealthValue.text = _health + "%";
        }

        [UsedImplicitly]
        public void Apply()
        {
            PlayerPrefs.SetInt(Health, _health);
            SceneManager.LoadScene("Main Menu");
        }

        [UsedImplicitly]
        public void Cancel()
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}
