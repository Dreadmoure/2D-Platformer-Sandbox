using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
    public class ManagerRoot : MonoBehaviour
    {
        public static ManagerRoot Instance { get; private set; }

        public GameSceneManager GameSceneManager { get; private set; }
        public GameAudioManager GameAudioManager { get; private set; }

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
            
            if (GameSceneManager == null)
                Debug.LogError("Managers: GameSceneManager not found as child.");

            if (GameAudioManager == null)
                Debug.LogError("Managers: GameAudioManager not found as child.");
        }
    }
}
