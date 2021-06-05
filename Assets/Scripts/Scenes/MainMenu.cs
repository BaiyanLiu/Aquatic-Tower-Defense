using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Scenes
{
    public class MainMenu : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene("Game");
        }

        public void Settings()
        {
            SceneManager.LoadScene("Settings");
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}
