using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CodeBase.Managers.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        
        [SerializeField] private Button newGameBtn;
        
        private void Start()
        {
            newGameBtn.onClick.AddListener(StartNewGame);
        }

        private void StartNewGame()
        {
            LevelManager.Instance.LoadNewGame();
        }
    }
}