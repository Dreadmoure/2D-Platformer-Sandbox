using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
    public class Manager : MonoBehaviour
    {
        public static Manager Instance;

        public SceneManager SceneManager { get; private set; }
        public AudioManager AudioManager { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            SceneManager = GetComponentInChildren<SceneManager>();
            AudioManager = GetComponentInChildren<AudioManager>();
        }
    }
}
