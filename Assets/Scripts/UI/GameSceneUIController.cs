using Player;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class GameSceneUIController : MonoBehaviour
    {
        private VisualElement _root;
        private Label _collectableValueLabel;
        
        private CollectableHandler _collectableHandler;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // Get the root of the UI document
            _root = GetComponent<UIDocument>().rootVisualElement;

            _collectableValueLabel = _root.Q<Label>("CollectableValueLabel");
            
            // Find the player's CollectableHandler
            _collectableHandler = FindAnyObjectByType<CollectableHandler>();

            if (_collectableHandler != null)
            {
                _collectableHandler.OnCollectableCountChanged += UpdateCollectableUI;

                // Optional: initialize UI immediately
                UpdateCollectableUI(_collectableHandler.GetCount());
            }
            else
            {
                Debug.LogWarning("No CollectableHandler found in scene.");
            }
        }
        
        private void OnDestroy()
        {
            if (_collectableHandler != null)
            {
                _collectableHandler.OnCollectableCountChanged -= UpdateCollectableUI;
            }
        }

        private void UpdateCollectableUI(int count)
        {
            _collectableValueLabel.text = _collectableHandler.GetCount().ToString();
        }
    }
}
