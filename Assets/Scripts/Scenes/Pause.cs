using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Scenes
{
    public class Pause : MonoBehaviour
    {
        [UsedImplicitly]
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Resume();
            }
        }

        [UsedImplicitly]
        public void Resume()
        {
            SceneManager.UnloadSceneAsync("Pause");
            GameState.IsPaused = false;
        }

        [UsedImplicitly]
        public void Restart()
        {
            SceneManager.LoadScene("Game");
        }

        [UsedImplicitly]
        public void MainMenu()
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}
