using System;
using Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class PauseMenuUIController : MonoBehaviour
    {
        private Button _resumeButton;
        private Button _backToMainMenuButton;
        private Button _quitButton;

        private VisualElement _pauseMenuPanel;
        
        private VisualElement _root;
        
        private InputTracker _inputTracker;
        
        private Action _unregisterResumeButton;
        private Action _unregisterBackToMainMenuButton;
        private Action _unregisterQuitButton;
        
        private void OnEnable()
        {
            ManagerRoot.Instance.InputTracker.Activate();
            _inputTracker = ManagerRoot.Instance.InputTracker;
            
            // Get the root of the UI document
            _root = GetComponent<UIDocument>().rootVisualElement;
            
            // Query buttons by name
            _resumeButton = _root.Q<Button>("ResumeButton");
            _backToMainMenuButton = _root.Q<Button>("BackToMainMenuButton");
            _quitButton = _root.Q<Button>("QuitButton");
            
            // Pause menu panel
            _pauseMenuPanel = _root.Q<VisualElement>("PauseMenuPanel");
            TogglePausePanel(false);
            
            // Safety checks
            if (_resumeButton == null || _backToMainMenuButton == null || _quitButton == null)
            {
                Debug.LogError("PauseMenuUIController: Button not found in UXML.");
                return;
            }

            // Register click callbacks
            _resumeButton.clicked += OnResumeClicked;
            _backToMainMenuButton.clicked += OnBackToMainMenuClicked;
            _quitButton.clicked += OnQuitClicked;
            
            // Register input tracking
            _unregisterResumeButton = _inputTracker.RegisterElementForInputTracking(_resumeButton);
            _unregisterBackToMainMenuButton = _inputTracker.RegisterElementForInputTracking(_backToMainMenuButton);
            _unregisterQuitButton = _inputTracker.RegisterElementForInputTracking(_quitButton);
            
            // Subscribe to pause toggled event
            ManagerRoot.Instance.GamePauseManager.OnPauseToggled += TogglePausePanel;
        }
        
        private void OnDisable()
        {
            ManagerRoot.Instance.InputTracker.Deactivate();

            _resumeButton.clicked -= OnResumeClicked;
            _backToMainMenuButton.clicked -= OnBackToMainMenuClicked;
            _quitButton.clicked -= OnQuitClicked;
            
            // Unregister input tracking
            _unregisterResumeButton?.Invoke();
            _unregisterBackToMainMenuButton?.Invoke();
            _unregisterQuitButton?.Invoke();
            
            // Unsubscribe from pause toggled event
            if (ManagerRoot.Instance.GamePauseManager != null)
                ManagerRoot.Instance.GamePauseManager.OnPauseToggled -= TogglePausePanel;
        }

        private void TogglePausePanel(bool isActive)
        {
            if (_pauseMenuPanel == null || _inputTracker == null)
                return;

            // Show/hide panel
            _pauseMenuPanel.style.display = isActive ? DisplayStyle.Flex : DisplayStyle.None;

            // Activate/deactivate InputTracker
            if (isActive)
                _inputTracker.Activate();
            else
                _inputTracker.Deactivate();
        }
        
        private void OnResumeClicked()
        {
            ManagerRoot.Instance.GamePauseManager.TogglePause();
        }
        
        private void OnBackToMainMenuClicked()
        {
            ManagerRoot.Instance.GameSceneManager.LoadMainMenuScene();
        }
        
        private void OnQuitClicked()
        {
            Application.Quit();
        }
    }
}
