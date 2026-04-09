using System;
using UnityEngine;

namespace Managers
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 100f;
        
        [Tooltip("How many lives the player starts with")]
        [SerializeField] private int livesCount = 3;
    
        [Tooltip("How many collectibles the player starts with")]
        [SerializeField] private int collectableCount = 0;
        
        public event Action<float, float> OnHealthChanged; 
        public event Action<int> OnCollectableCountChanged;
        public event Action<int> OnLivesCountChanged;
    
        public float CurrentHealth { get; private set; }
        public float MaxHealth { get; private set; }
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
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
            OnCollectableCountChanged?.Invoke(CollectableCount);
            OnLivesCountChanged?.Invoke(LivesCount);
        }

        public void UpdateHealth(float value)
        {
            CurrentHealth += value;
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
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

        public void ResetHealth()
        {
            CurrentHealth = MaxHealth;
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }

        private void ResetValues()
        {
            MaxHealth = maxHealth;
            CurrentHealth = MaxHealth;
            LivesCount = livesCount;
            CollectableCount = collectableCount;
        }
    }
}
