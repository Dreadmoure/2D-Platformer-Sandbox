using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class DeathFlowManager : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;

        private PlayerManager _playerManager;
        private Transform _spawnPoint;
        private GameObject _currentPlayer;
        private HashSet<string> _gameScenes;

        private void Awake()
        {
            _playerManager = ManagerRoot.Instance.PlayerManager;
        
            _gameScenes = new HashSet<string>(
                ManagerRoot.Instance.GameSceneManager.GetGameSceneNames()
            );
        
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (!IsGameplayScene(scene.name))
                return;
        
            SpawnPlayer();
        }
    
        private bool IsGameplayScene(string sceneName)
        {
            return _gameScenes.Contains(sceneName);
        }
    
        private void SpawnPlayer()
        {
            if (_spawnPoint == null)
            {
                GameObject sp = GameObject.FindGameObjectWithTag("PlayerSpawnPoint");
                if (sp != null)
                {
                    _spawnPoint = sp.transform.GetChild(0);
                }
                    
            }

            _currentPlayer = Instantiate(playerPrefab, _spawnPoint.position, _spawnPoint.rotation);
        }

        public void HandlePlayerDeath()
        {
            Debug.Log("HandlePlayerDeath FRAME: " + Time.frameCount);
            string sceneName = SceneManager.GetActiveScene().name;

            if (!IsGameplayScene(sceneName))
                return;
        
            // Destroy current player FIRST
            if (_currentPlayer != null)
            {
                Destroy(_currentPlayer);
                _currentPlayer = null;
            }

            if (_playerManager.LivesCount > 0)
            {
                Respawn();
            }
            else
            {
                GameOver();
            }
        }

        private void Respawn()
        {
            _playerManager.ResetHealth();

            SpawnPlayer();
        }

        private void GameOver()
        {
            if (_currentPlayer != null)
                Destroy(_currentPlayer);

            Debug.Log("Game over");
            ManagerRoot.Instance.GameSceneManager.LoadGameOverMenuScene();
        }
        
        public void DisablePlayerColliders()
        {
            if (_currentPlayer == null) return;

            var colliders = _currentPlayer.GetComponentsInChildren<Collider2D>();

            foreach (var col in colliders)
            {
                col.enabled = false;
            }
        }
    }
}
