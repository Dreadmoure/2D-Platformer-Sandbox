using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerSpawnPoint : MonoBehaviour
    {
        [SerializeField]  private GameObject playerPrefab;
    
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

        public void RespawnPlayer()
        {
            if (ManagerRoot.Instance.PlayerManager.LivesCount > 0)
            {
                Instantiate(playerPrefab, transform.position, transform.rotation);
                ManagerRoot.Instance.PlayerManager.ResetHealth();
            }
            else
            {
                ManagerRoot.Instance.GameSceneManager.LoadGameOverMenuScene();
            }
        }
    }
}
