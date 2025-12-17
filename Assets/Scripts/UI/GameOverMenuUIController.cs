using Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class GameOverMenuUIController : MonoBehaviour
    {
        private Button _retryButton;
        private Button _backToMainMenuButton;
        private Button _quitButton;

        private VisualElement _root;
    
        private void OnEnable()
        {
            // Get the root of the UI document
            _root = GetComponent<UIDocument>().rootVisualElement;
            
            // Query buttons by name
            _retryButton = _root.Q<Button>("RetryButton");
            _backToMainMenuButton = _root.Q<Button>("BackToMainMenuButton");
            _quitButton = _root.Q<Button>("QuitButton");
            
            // Safety checks
            if (_retryButton == null || _backToMainMenuButton == null || _quitButton == null)
            {
                Debug.LogError("GameOverMenuUIController: Button not found in UXML.");
                return;
            }

            // Register click callbacks
            _retryButton.clicked += OnRetryClicked;
            _backToMainMenuButton.clicked += OnBackToMainMenuClicked;
            _quitButton.clicked += OnQuitClicked;

            // Optional: set initial keyboard/controller focus
            _retryButton.Focus();
        }
        
        /// <summary>
        /// called when switching scenes or quitting application
        /// </summary>
        private void OnDisable()
        {
            // Always unregister callbacks
            _retryButton.clicked -= OnRetryClicked;
            _retryButton.clicked -= OnBackToMainMenuClicked;
            _quitButton.clicked -= OnQuitClicked;
        }
        
        private void OnRetryClicked()
        {
            ManagerRoot.Instance.GameSceneManager.LoadScene(1);
        }
        
        private void OnBackToMainMenuClicked()
        {
            ManagerRoot.Instance.GameSceneManager.LoadFirstScene();
        }

        private void OnQuitClicked()
        {
            Application.Quit();
        }
    }
}
