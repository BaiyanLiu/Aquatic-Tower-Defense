using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Scenes
{
    public sealed class Pause : MonoBehaviour
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
        public void Save()
        {
            GameState.Instance.Save();
            SceneManager.UnloadSceneAsync("Pause");
        }

        [UsedImplicitly]
        public void Load()
        {
            GameState.Instance.Load();
            SceneManager.UnloadSceneAsync("Pause");
        }

        [UsedImplicitly]
        public void Resume()
        {
            GameState.Instance.IsPaused = false;
            SceneManager.UnloadSceneAsync("Pause");
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
