using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Scenes
{
    public sealed class GameOver : MonoBehaviour
    {
        [UsedImplicitly]
        public void Load()
        {
            PlayerPrefs.SetInt(Scenes.Settings.Load, 1);
            SceneManager.LoadScene("Game");
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
