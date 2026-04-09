using Managers;
using Player;
using UnityEngine;

namespace Dangers
{
    public class FallPit : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }
            
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
