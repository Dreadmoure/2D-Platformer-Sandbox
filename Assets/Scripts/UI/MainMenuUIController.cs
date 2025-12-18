using System;
using Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class MainMenuUIController : MonoBehaviour
    {
        private Button _newGameButton;
        private Button _quitButton;

        private VisualElement _root;
        
        private InputTracker _inputTracker;
        
        private Action _unregisterNewGameButton;
        private Action _unregisterQuitButton;

        private void OnEnable()
        {
            ManagerRoot.Instance.InputTracker.Activate();
            _inputTracker = ManagerRoot.Instance.InputTracker;
            
            // Get the root of the UI document
            _root = GetComponent<UIDocument>().rootVisualElement;
            
            // Query buttons by name
            _newGameButton = _root.Q<Button>("NewGameButton");
            _quitButton = _root.Q<Button>("QuitButton");
            
            // Safety checks
            if (_newGameButton == null || _quitButton == null)
            {
                Debug.LogError("MainMenuUIController: Button not found in UXML.");
                return;
            }

            // Register click callbacks
            _newGameButton.clicked += OnNewGameClicked;
            _quitButton.clicked += OnQuitClicked;
            
            // Register input tracking
            _unregisterNewGameButton = _inputTracker.RegisterElementForInputTracking(_newGameButton);
            _unregisterQuitButton = _inputTracker.RegisterElementForInputTracking(_quitButton);
        }
        
        /// <summary>
        /// called when switching scenes or quitting application
        /// </summary>
        private void OnDisable()
        {
            ManagerRoot.Instance.InputTracker.Deactivate();
            if (_newGameButton == null || _quitButton == null)
                return;

            _newGameButton.clicked -= OnNewGameClicked;
            _quitButton.clicked -= OnQuitClicked;
            
            // Unregister input tracking
            _unregisterNewGameButton?.Invoke();
            _unregisterQuitButton?.Invoke();
        }
        
        // -------------------------
        // Button callbacks
        // -------------------------
        
        private void OnNewGameClicked()
        {
            ManagerRoot.Instance.GameSceneManager.LoadNextScene();
        }

        private void OnQuitClicked()
        {
            Application.Quit();
        }
    }
}
