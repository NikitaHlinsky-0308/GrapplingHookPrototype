using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Managers
{
    public class GameManager : MonoBehaviour
    {
        private void Start()
        {
            QualitySettings.vSyncCount = 0; // Set vSyncCount to 0 so that using .targetFrameRate is enabled.
            Application.targetFrameRate = 120;
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}