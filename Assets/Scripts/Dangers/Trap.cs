using Managers;
using Player;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [Tooltip("The amount of damage done to the player")]
    [SerializeField] private float damage = 50f;
    [SerializeField] private bool isActive = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isActive)
        {
            if (other.CompareTag("Player"))
            {
                isActive = false;
                ManagerRoot.Instance.PlayerManager.UpdateHealth(-damage);

                if (ManagerRoot.Instance.PlayerManager.CurrentHealth <= 0)
                {
                    // Destroy player
                    Destroy(other.gameObject);
                    
                    // Decrease player lives
                    ManagerRoot.Instance.PlayerManager.ChangeLivesCount(-1);
                    
                    // Call on spawner to spawn player -> let it check if player has lives left otherwise change scene to gameover scene
                    GameObject playerSpawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawnPoint");
            
                    if (playerSpawnPoint == null)
                    {
                        Debug.LogError("No player spawn point object found in scene");
                        return;
                    }
            
                    PlayerSpawnPoint component = playerSpawnPoint.GetComponent<PlayerSpawnPoint>();
            
                    component.RespawnPlayer();
                }
            }
        }
        
    }
}
