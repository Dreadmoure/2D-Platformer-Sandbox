using System;
using Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class MainMenuUIController : MonoBehaviour
    {
        private VisualElement _root;
        
        private Button _newGameButton;
        private Button _quitButton;

        private void OnEnable()
        {
            // Get the root of the UI document
            _root = GetComponent<UIDocument>().rootVisualElement;
            
            // Query buttons by name
            _newGameButton = _root.Q<Button>("NewGameButton");
            _quitButton = _root.Q<Button>("QuitButton");
            
            if (_newGameButton == null || _quitButton == null)
            {
                Debug.LogError("MainMenuUIController: One or more buttons not found in UXML.");
                return;
            }

            // Register click callbacks
            _newGameButton.clicked += OnNewGameClicked;
            _quitButton.clicked += OnQuitClicked;
        }
        
        /// <summary>
        /// called when switching scenes or quitting application
        /// </summary>
        private void OnDisable()
        {
            if (_newGameButton != null)
                _newGameButton.clicked -= OnNewGameClicked;

            if (_quitButton != null)
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
