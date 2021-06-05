using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Scenes
{
    public class MainMenu : MonoBehaviour
    {
        [UsedImplicitly]
        public void StartGame()
        {
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
