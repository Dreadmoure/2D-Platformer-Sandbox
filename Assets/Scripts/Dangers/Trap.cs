using Managers;
using UnityEngine;

namespace Dangers
{
    public class Trap : MonoBehaviour
    {
        [Tooltip("The amount of damage done to the player")]
        [SerializeField] private float damage = 50f;
        [SerializeField] private bool isActive = true;
        
        Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isActive)
            {
                if (other.CompareTag("Player"))
                {
                    isActive = false;
                    
                    ManagerRoot.Instance.GameAudioManager.PlaySfx(GameAudioManager.SfxType.Trap);
                    
                    animator.SetTrigger("TrapTriggered");
                    
                    ManagerRoot.Instance.PlayerManager.TakeDamage(damage);
                }
            }
        
        }
    }
}
