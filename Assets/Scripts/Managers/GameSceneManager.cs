 using System.Collections;
 using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameSceneManager : MonoBehaviour
    {
        [SerializeField] private SceneAsset mainMenuScene;
        
        [SerializeField] private List<SceneAsset> gameScenes;
        
        [SerializeField] private SceneAsset winMenuScene;
        
        [SerializeField] private SceneAsset gameOverMenuScene;

        private int _currentSceneIndex;
        private bool _isLoadingScene = false;
        
        #region Scene Name Getters
        public string GetMainMenuSceneName() => mainMenuScene?.name;
        public string GetGameOverMenuSceneName() => gameOverMenuScene?.name;
        public string GetWinMenuSceneName() => winMenuScene?.name;
        public List<string> GetGameSceneNames()
        {
            var names = new List<string>();
            foreach (var s in gameScenes)
            {
                if (s != null) names.Add(s.name);
            }
            return names;
        }
        #endregion
        
        private void Awake()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _currentSceneIndex = gameScenes.FindIndex(s => s.name == scene.name);
            
            // Reset loading lock after scene is loaded
            _isLoadingScene = false;
        }

        public void LoadMainMenuScene()
        {
            TryLoadScene(mainMenuScene);
        }
        
        public void LoadWinMenuScene()
        {
            TryLoadScene(winMenuScene);
        }
        
        public void LoadGameOverMenuScene()
        {
            TryLoadScene(gameOverMenuScene);
        }
        
        public void LoadGameScene(int index)
        {
            if (index < 0 || index >= gameScenes.Count)
            {
                Debug.LogError($"Invalid scene index: {index}");
                return;
            }

            TryLoadScene(gameScenes[index]);
        }

        public void LoadNextScene()
        {
            if (gameScenes == null || gameScenes.Count == 0)
            {
                Debug.LogError("Game scenes list is empty.");
                return;
            }

            if (_currentSceneIndex < 0)
            {
                TryLoadScene(gameScenes[0]);
                return;
            }

            if (_currentSceneIndex + 1 >= gameScenes.Count)
            {
                LoadWinMenuScene();
                return;
            }

            _currentSceneIndex++;
            TryLoadScene(gameScenes[_currentSceneIndex]);
        }
        
        private void TryLoadScene(SceneAsset sceneAsset)
        {
            if (_isLoadingScene)
            {
                return;
            }

            if (sceneAsset == null)
            {
                return;
            }

            _isLoadingScene = true;

            StartCoroutine(LoadSceneSafely(sceneAsset.name));
        }
        
        private IEnumerator LoadSceneSafely(string sceneName)
        {
            // Wait for physics to fully finish
            yield return new WaitForFixedUpdate();

            // Wait until end of frame (render + destruction done)
            yield return new WaitForEndOfFrame();

            // Extra safety frame
            yield return null;

            SceneManager.LoadScene(sceneName);
        }
        
        #region Utility
        public List<string> GetNonPausableScenesByName()
        {
            var result = new List<string>();

            if (mainMenuScene != null)
                result.Add(mainMenuScene.name);

            if (winMenuScene != null)
                result.Add(winMenuScene.name);

            if (gameOverMenuScene != null)
                result.Add(gameOverMenuScene.name);

            return result;
        }
        #endregion
    }
}
