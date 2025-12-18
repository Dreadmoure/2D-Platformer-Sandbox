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
            
            // Not currently in a game scene → start game
            if (_currentSceneIndex < 0)
            {
                _currentSceneIndex = 0;
                SceneManager.LoadScene(gameScenes[0].name);
                return;
            }
            
            // Already in a game scene → move forward
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
            SceneManager.LoadScene(gameScenes[index].name);
        }

        public void LoadGameOverMenuScene()
        {
            SceneManager.LoadScene(gameOverMenuScene.name);
        }
        
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _currentSceneIndex = gameScenes.FindIndex(s => s.name == scene.name);
        }
    }
}
