using System;
using UnityEngine;

namespace Managers
{
    public class PlayerManager : MonoBehaviour
    {
        [Tooltip("How many lives the player starts with")]
        [SerializeField] private int livesCount = 3;
    
        [Tooltip("How many collectibles the player starts with")]
        [SerializeField] private int collectableCount = 0;
        
        public event Action<int> OnCollectableCountChanged;
        public event Action<int> OnLivesCountChanged;
    
        public int LivesCount { get; private set; }
        public int CollectableCount { get; private set; }
        
        private void Awake()
        {
            // Initialize once before any Start() methods run
            ResetValues();
        }

        private void Start()
        {
            // Optional: notify anything that subscribed in Start()
            OnCollectableCountChanged?.Invoke(CollectableCount);
            OnLivesCountChanged?.Invoke(LivesCount);
        }
        
        public void ChangeCollectableCount(int value)
        {
            CollectableCount += value;
            OnCollectableCountChanged?.Invoke(CollectableCount);
        }

        public void ChangeLivesCount(int value)
        {
            LivesCount += value;
            OnLivesCountChanged?.Invoke(LivesCount);
        }

        public void ResetValues()
        {
            LivesCount = livesCount;
            CollectableCount = collectableCount;
        }
    }
}
