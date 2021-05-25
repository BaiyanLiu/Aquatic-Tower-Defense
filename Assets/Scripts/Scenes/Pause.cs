using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Scenes
{
    public class Pause : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Resume();
            }
        }

        public void Resume()
        {
            SceneManager.UnloadSceneAsync("Pause");
            GameState.IsPaused = false;
        }

        public void Restart()
        {
            SceneManager.LoadScene("Game");
        }

        public void MainMenu()
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}
