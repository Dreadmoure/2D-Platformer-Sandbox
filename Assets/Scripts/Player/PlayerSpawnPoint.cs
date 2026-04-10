using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerSpawnPoint : MonoBehaviour
    {
        [SerializeField]  private GameObject playerPrefab;

        private void OnEnable()
        {
            ManagerRoot.Instance.PlayerManager.OnPlayerDied -= RespawnPlayer;
            
            ManagerRoot.Instance.PlayerManager.OnPlayerDied += RespawnPlayer;
        }
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
                return;

            if (playerPrefab == null)
            {
                Debug.LogError("PlayerSpawnPoint: No Player prefab assigned!", this);
                return;
            }

            Instantiate(playerPrefab, transform.position, transform.rotation);
        }
        
        private void OnDisable()
        {
            if (ManagerRoot.Instance != null)
                ManagerRoot.Instance.PlayerManager.OnPlayerDied -= RespawnPlayer;
        }

        private void RespawnPlayer()
        {
            var playerManager = ManagerRoot.Instance.PlayerManager;

            if (playerManager.LivesCount > 0)
            {
                Instantiate(playerPrefab, transform.position, transform.rotation);
                playerManager.ResetHealth();
            }
            else
            {
                ManagerRoot.Instance.GameSceneManager.LoadGameOverMenuScene();
            }
        }
    }
}
