using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
    public class ManagerRoot : MonoBehaviour
    {
        public static ManagerRoot Instance { get; private set; }

        public GameSceneManager GameSceneManager { get; private set; }
        public GameAudioManager GameAudioManager { get; private set; }
        
        public GamePauseManager GamePauseManager { get; private set; }
        
        public InputTracker InputTracker { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            GameSceneManager = GetComponentInChildren<GameSceneManager>();
            GameAudioManager = GetComponentInChildren<GameAudioManager>();
            GamePauseManager = GetComponentInChildren<GamePauseManager>();
            InputTracker = GetComponentInChildren<InputTracker>();
            
            if (GameSceneManager == null)
                Debug.LogError("Managers: GameSceneManager not found as child.");

            if (GameAudioManager == null)
                Debug.LogError("Managers: GameAudioManager not found as child.");
            
            if (GamePauseManager == null)
                Debug.LogError("Managers: GamePauseManager not found as child.");
            
            if (InputTracker == null)
                Debug.LogError("Managers: InputTracker not found as child.");
        }
    }
}
