using UnityEngine;

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
