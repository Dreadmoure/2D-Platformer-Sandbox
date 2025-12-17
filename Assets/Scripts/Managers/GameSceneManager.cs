using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Managers
{
    public class GameSceneManager : MonoBehaviour
    {
        [SerializeField] private List<SceneAsset> scenes;

        private int _currentSceneIndex;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            var activeSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            _currentSceneIndex = scenes.FindIndex(
                s => s.name == activeSceneName
            );
        }

        /// <summary>
        /// Loads the next scene on the scenes list
        /// </summary>
        public void LoadNextScene()
        {
            if (_currentSceneIndex < 0)
            {
                Debug.LogError("Current scene not found in scene list.");
                return;
            }

            if (_currentSceneIndex + 1 >= scenes.Count)
            {
                Debug.Log("No more scenes to load.");
                return;
            }

            _currentSceneIndex++;
            UnityEngine.SceneManagement.SceneManager.LoadScene(scenes[_currentSceneIndex].name);
        }

        /// <summary>
        /// Load a specific scene based on the index of scenes list
        /// </summary>
        /// <param name="index">index on scenes list starting from 0</param>
        public void LoadScene(int index)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(index);
        }

        /// <summary>
        /// Loads the first scene on the list
        /// </summary>
        public void LoadFirstScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

        /// <summary>
        /// Loads the last scene on the list
        /// </summary>
        public void LoadLastScene()
        {
            if (scenes?.Count > 0)
            {
                _currentSceneIndex = scenes.Count - 1;
                UnityEngine.SceneManagement.SceneManager.LoadScene(scenes[_currentSceneIndex].name);
            }
            else
            {
                Debug.LogError("Scenes list is empty.");
            }
        }
    }
}
