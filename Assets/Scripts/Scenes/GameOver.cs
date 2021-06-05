using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Scenes
{
    public class GameOver : MonoBehaviour
    {
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
