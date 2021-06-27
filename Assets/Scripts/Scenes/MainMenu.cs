using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Scenes
{
    public sealed class MainMenu : MonoBehaviour
    {
        [UsedImplicitly]
        private void Start()
        {
            if (!PlayerPrefs.HasKey(Scenes.Settings.Health))
            {
                PlayerPrefs.SetInt(Scenes.Settings.Health, 100);
            }
            PlayerPrefs.SetInt(Scenes.Settings.Load, 0);
        }

        [UsedImplicitly]
        public void StartGame()
        {
            SceneManager.LoadScene("Game");
        }

        [UsedImplicitly]
        public void Load()
        {
            PlayerPrefs.SetInt(Scenes.Settings.Load, 1);
            SceneManager.LoadScene("Game");
        }

        [UsedImplicitly]
        public void Settings()
        {
            SceneManager.LoadScene("Settings");
        }

        [UsedImplicitly]
        public void Exit()
        {
            Application.Quit();
        }
    }
}
