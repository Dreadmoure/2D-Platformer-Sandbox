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

        private HashSet<string> _nonPausableSceneNames;

        private void Awake()
        {
            _nonPausableSceneNames = new HashSet<string>(ManagerRoot.Instance.GameSceneManager.GetNonPausableScenesByName()); 
            
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
            if (_nonPausableSceneNames == null)
            {
                Debug.LogWarning("GamePauseManager: Non-pausable scenes not initialized yet.");
                return false; // safest default
            }

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
