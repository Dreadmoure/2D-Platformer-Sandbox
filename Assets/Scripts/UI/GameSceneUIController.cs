using Managers;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI
{
    public class GameSceneUIController : MonoBehaviour
    {
        private Label _collectableValueLabel;
        private Label _livesValueLabel;
        
        private PlayerManager _playerManager;
    
        private void Awake()
        {
            // Subscribe to player events
            _playerManager = ManagerRoot.Instance.PlayerManager;
            _playerManager.OnCollectableCountChanged += UpdateCollectibleCountUI;
            _playerManager.OnLivesCountChanged += UpdateLivesCountUI;

            // Listen for scene changes
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // Bind labels for the first loaded scene
            BindLabels();
        }
        
        private void BindLabels()
        {
            var uiDocument = FindAnyObjectByType<UIDocument>();
            if (uiDocument == null)
            {
                Debug.LogWarning("GameSceneUIController: No UIDocument found in the current scene.");
                _collectableValueLabel = null;
                _livesValueLabel = null;
                return;
            }

            // Schedule the binding for the next frame, ensures rootVisualElement is ready
            uiDocument.rootVisualElement.schedule.Execute(_ =>
            {
                _collectableValueLabel = uiDocument.rootVisualElement.Q<Label>("CollectableValueLabel");
                _livesValueLabel = uiDocument.rootVisualElement.Q<Label>("LivesValueLabel");

                if (_collectableValueLabel == null)
                    Debug.LogWarning("GameSceneUIController: Could not find CollectableValueLabel in UIDocument.");

                if (_livesValueLabel == null)
                    Debug.LogWarning("GameSceneUIController: Could not find LivesValueLabel in UIDocument.");

                // Update UI immediately after binding
                UpdateCollectibleCountUI(_playerManager.CollectableCount);
                UpdateLivesCountUI(_playerManager.LivesCount);
            });
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Rebind labels whenever a new scene loads
            BindLabels();
        }
        
        private void OnDestroy()
        {
            _playerManager.OnCollectableCountChanged -= UpdateCollectibleCountUI;
            _playerManager.OnLivesCountChanged -= UpdateLivesCountUI;
        }

        private void UpdateCollectibleCountUI(int value)
        {
            if (_collectableValueLabel != null)
                _collectableValueLabel.text = value.ToString();
        }
        
        private void UpdateLivesCountUI(int value)
        {
            if (_livesValueLabel != null)
                _livesValueLabel.text = value.ToString();
        }
    }
}
