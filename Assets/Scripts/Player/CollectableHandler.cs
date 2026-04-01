using System;
using UnityEngine;

namespace Player
{
    public class CollectableHandler : MonoBehaviour
    {
        private int _collectableCount;
        
        // Event: sends the NEW total count
        public event Action<int> OnCollectableCountChanged;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _collectableCount = 0;
            
            // Fire once at start so UI shows the initial value immediately
            OnCollectableCountChanged?.Invoke(_collectableCount);
        }

        public void AddCount(int value)
        {
            _collectableCount += value;
            OnCollectableCountChanged?.Invoke(_collectableCount);
        }

        public int GetCount()
        {
            return _collectableCount;
        }
    }
}
