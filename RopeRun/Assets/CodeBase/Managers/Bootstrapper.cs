using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CodeBase.Managers
{
    public class Bootstrapper : MonoBehaviour
    {

        [Header("Scene settings")] 
        [SerializeField] private string _gameplaySceneName;

        [SerializeField] private GameObject _loadingScreen;
        [SerializeField] private Slider _loadingBar;

        void Start()
        {
            LoadScene(_gameplaySceneName);
        }

        public async void LoadScene(string sceneName)
        {
            var scene = SceneManager.LoadSceneAsync(sceneName);
            if (scene != null)
            {
                scene.allowSceneActivation = false;

                _loadingScreen.SetActive(true);

                do
                {
                    await Task.Delay(100);
                    _loadingBar.value = scene.progress;
                    //} while (!scene.isDone);
                } while (scene.progress < 0.9f);


                scene.allowSceneActivation = true;
                _loadingScreen.SetActive(false);
            }
        }
    }
}