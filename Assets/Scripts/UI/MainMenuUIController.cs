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

        private void OnEnable()
        {
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

            // Optional: set initial keyboard/controller focus
            _newGameButton.Focus();
        }
        
        /// <summary>
        /// called when switching scenes or quitting application
        /// </summary>
        private void OnDisable()
        {
            // Always unregister callbacks
            _newGameButton.clicked -= OnNewGameClicked;
            _quitButton.clicked -= OnQuitClicked;
        }
        
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
