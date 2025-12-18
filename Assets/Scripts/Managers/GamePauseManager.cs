using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GamePauseManager : MonoBehaviour
    {
        public event Action<bool> OnPauseToggled;
        
        private bool _isPaused;
        
        public bool IsPaused => _isPaused;
        
        [Header("Non-pausable Scenes (assign in Inspector)")]
        [SerializeField] private List<SceneAsset> nonPausableSceneAssets = new List<SceneAsset>();
        
        private List<string> _nonPausableSceneNames = new List<string>();
        
        private void Awake()
        {
            // Convert SceneAssets to names for runtime checks
            _nonPausableSceneNames.Clear();
            foreach (var sceneAsset in nonPausableSceneAssets)
            {
                if (sceneAsset != null)
                    _nonPausableSceneNames.Add(sceneAsset.name);
            }
            
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (!CanPauseInScene(scene.name))
                ForceUnpause();
        }
        
        public void TogglePause()
        {
            if (!CanPause()) return;

            _isPaused = !_isPaused;
            Time.timeScale = _isPaused ? 0f : 1f;

            // Activate or deactivate InputTracker depending on pause state
            if (_isPaused)
                ManagerRoot.Instance.InputTracker?.Activate();
            else
                ManagerRoot.Instance.InputTracker?.Deactivate();

            OnPauseToggled?.Invoke(_isPaused);
        }
        
        public bool CanPause()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            return CanPauseInScene(sceneName);
        }
        
        private bool CanPauseInScene(string sceneName)
        {
            return !_nonPausableSceneNames.Contains(sceneName);
        }
        
        public void ForceUnpause()
        {
            if (!_isPaused) return;
            _isPaused = false;
            Time.timeScale = 1f;
            
            ManagerRoot.Instance.InputTracker?.Deactivate();
            ManagerRoot.Instance.InputTracker?.ApplyCursorVisibility(); // force correct cursor
            
            OnPauseToggled?.Invoke(false);
        }
    }
}
