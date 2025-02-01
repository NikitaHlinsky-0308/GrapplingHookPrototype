using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Managers
{
    public class LevelManager : MonoBehaviour
    {
        public enum Scene
        {
            BOOTSCENE,
            MainMenu,
            Gameplay_1,
            Gameplay_2,
        }

        public static LevelManager Instance; // Синглтон для доступа к менеджеру


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                //DontDestroyOnLoad(gameObject); // Сохраняем объект между сценами
            }
            else
            {
                Destroy(gameObject);
            }
        }


        public void LoadNextLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        

        // Метод для перезапуска текущего уровня
        public void RestartLevel()
        {
            Debug.Log("restart button pressed");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // Метод для загрузки конкретного уровня
        public void LoadLevel(Scene scene)
        {
            SceneManager.LoadScene(scene.ToString());
        }

        public void LoadNewGame()
        {
            SceneManager.LoadScene(Scene.Gameplay_1.ToString());
        }
        
        public void LoadMainMenu()
        {
            SceneManager.LoadScene(Scene.MainMenu.ToString());
        }
    }
}