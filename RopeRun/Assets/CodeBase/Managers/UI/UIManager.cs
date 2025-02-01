using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CodeBase.Managers.UI
{
    public class UIManager : MonoBehaviour
    {
        // Ссылки на основные UI элементы
        [FormerlySerializedAs("mainMenuPanel")] [Header("UI Panels")]
        public GameObject finishPanel;


        [Header("Popup Messages")] public GameObject popupMessagePrefab;
        public Transform popupMessageContainer;

        private int currentScore = 0;
        private int currentHealth = 100;

        // Синглтон для глобального доступа (опционально)
        public static UIManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject); // Если UIManager должен сохраняться между сценами
        }

        private void Start()
        {
            HideAllPanels();
        }

        private void HideAllPanels()
        {
            finishPanel.SetActive(false);
        }

        // Методы для управления панелями
        public void ShowFinishPanel()
        {
            finishPanel.SetActive(true);
        }


        // Метод для отображения всплывающих сообщений
        public void ShowPopupMessage(string message, float duration = 2f)
        {
            if (popupMessagePrefab == null || popupMessageContainer == null) return;

            GameObject popup = Instantiate(popupMessagePrefab, popupMessageContainer);
            Text popupText = popup.GetComponentInChildren<Text>();
            if (popupText != null)
            {
                popupText.text = message;
            }

            Destroy(popup, duration);
        }
    }
}