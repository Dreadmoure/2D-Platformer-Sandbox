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
        
        [SerializeField] private SceneAsset gameOverMenuScene;

        private int _currentSceneIndex;
        
        public string GetMainMenuSceneName() => mainMenuScene?.name;
        public string GetGameOverMenuSceneName() => gameOverMenuScene?.name;
        public List<string> GetGameSceneNames()
        {
            var names = new List<string>();
            foreach (var s in gameScenes)
            {
                if (s != null) names.Add(s.name);
            }
            return names;
        }
        
        private void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _currentSceneIndex = gameScenes.FindIndex(s => s.name == scene.name);
        }

        public void LoadMainMenuScene()
        {
            if (mainMenuScene == null)
            {
                Debug.LogError("No SceneAsset assigned in Main Menu Scene field");
                return;
            }

            SceneManager.LoadScene(mainMenuScene.name);
        }

        /// <summary>
        /// Loads the next scene on the gameScenes list if there are none, go to gameover menu scene
        /// </summary>
        public void LoadNextScene()
        {
            if (gameScenes == null || gameScenes.Count == 0)
            {
                Debug.LogError("Game scenes list is empty.");
                return;
            }

            if (_currentSceneIndex < 0)
            {
                _currentSceneIndex = 0;
                SceneManager.LoadScene(gameScenes[0].name);
                return;
            }

            if (_currentSceneIndex + 1 >= gameScenes.Count)
            {
                LoadGameOverMenuScene();
                return;
            }

            _currentSceneIndex++;
            SceneManager.LoadScene(gameScenes[_currentSceneIndex].name);
        }

        /// <summary>
        /// Load a specific scene based on the index of scenes list
        /// </summary>
        /// <param name="index">index on scenes list starting from 0</param>
        public void LoadGameScene(int index)
        {
            if (index < 0 || index >= gameScenes.Count)
            {
                Debug.LogError($"Invalid scene index: {index}");
                return;
            }

            SceneManager.LoadScene(gameScenes[index].name);
        }

        public void LoadGameOverMenuScene()
        {
            if (gameOverMenuScene == null)
            {
                Debug.LogError("No SceneAsset assigned in Game Over Menu Scene field");
                return;
            }

            SceneManager.LoadScene(gameOverMenuScene.name);
        }

        public List<string> GetNonPausableScenesByName()
        {
            var result = new List<string>();

            if (mainMenuScene != null)
            {
                result.Add(mainMenuScene.name);
            }
            else
            {
                Debug.LogWarning("Main Menu Scene is not assigned.");
            }
            
            if (gameOverMenuScene != null)
            {
                result.Add(gameOverMenuScene.name);
            }
            else
            {
                Debug.LogWarning("Game Over Menu Scene is not assigned.");
            }
            
            return result;
        }
    }
}
