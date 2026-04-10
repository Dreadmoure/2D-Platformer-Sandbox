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
        public event Action OnPlayerDied;
    
        public float CurrentHealth { get; private set; }
        public float MaxHealth { get; private set; }
        public int LivesCount { get; private set; }
        public int CollectableCount { get; private set; }
        
        private bool _isDead = false;
        
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

        public void TakeDamage(float value)
        {
            Debug.Log("A - TakeDamage ENTER");
            if (_isDead) return; // 🛑 prevent repeated death
            
            CurrentHealth -= value;
            
            Debug.Log("B - After damage calc");
            
            if (CurrentHealth <= 0)
            {
                Debug.Log("B - After damage calc");
                ManagerRoot.Instance.GameAudioManager.PlaySfx(GameAudioManager.SfxType.TakeDamage);
                CurrentHealth = 0;
                _isDead = true;

                LivesCount--;
                
                Debug.Log("D - Before event");
                OnPlayerDied?.Invoke(); // Trigger respawn system
                Debug.Log("E - After event");
                OnLivesCountChanged?.Invoke(LivesCount);
            }
            Debug.Log("E - After event");
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }
        
        public bool Heal(float amount)
        {
            if (_isDead) return false;
            
            if (CurrentHealth >= MaxHealth)
                return false; // nothing to heal

            float oldHealth = CurrentHealth;

            CurrentHealth += amount;

            if (CurrentHealth > MaxHealth)
                CurrentHealth = MaxHealth;

            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

            return CurrentHealth > oldHealth; // ✅ actually healed
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
            _isDead = false;
            CurrentHealth = MaxHealth;
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }

        public void ResetValues()
        {
            _isDead = false;
            MaxHealth = maxHealth;
            CurrentHealth = MaxHealth;
            LivesCount = livesCount;
            CollectableCount = collectableCount;
        }
    }
}
