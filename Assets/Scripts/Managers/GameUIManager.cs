using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenuUI;
        [SerializeField] private GameObject gameSceneUI;
        [SerializeField] private GameObject gameOverMenuUI;
        
        private Dictionary<string, GameObject> _sceneToUIMap;

        private void Awake()
        {
            // Map scene names to UI GameObjects
            _sceneToUIMap = new Dictionary<string, GameObject>
            {
                { ManagerRoot.Instance.GameSceneManager.GetMainMenuSceneName(), mainMenuUI },
                { ManagerRoot.Instance.GameSceneManager.GetGameOverMenuSceneName(), gameOverMenuUI }
            };
            
            // Map game scenes
            foreach (var sceneName in ManagerRoot.Instance.GameSceneManager.GetGameSceneNames())
            {
                _sceneToUIMap[sceneName] = gameSceneUI;
            }
            
            // Subscribe to sceneLoaded
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        private void Start()
        {
            // Set UI for the currently active scene
            UpdateUI(SceneManager.GetActiveScene().name);
        }
        
        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            UpdateUI(scene.name);
        }

        private void UpdateUI(string sceneName)
        {
            // Deactivate all first
            mainMenuUI.SetActive(false);
            gameSceneUI.SetActive(false);
            gameOverMenuUI.SetActive(false);

            // Activate the right UI if mapped
            if (_sceneToUIMap.TryGetValue(sceneName, out var ui))
            {
                ui.SetActive(true);
            }
        }
        
        // Optional helper methods for external calls
        public void HideAllUI()
        {
            mainMenuUI.SetActive(false);
            gameSceneUI.SetActive(false);
            gameOverMenuUI.SetActive(false);
        }
    }
}
