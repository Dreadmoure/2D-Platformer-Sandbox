using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Managers
{
    public class GameAudioManager : MonoBehaviour
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;
        
        [Header("Volume")]
        [Range(0f, 1f)] public float masterVolume = 1f;
        [Range(0f, 1f)] public float musicVolume = 1f;
        [Range(0f, 1f)] public float sfxVolume = 1f;

        public enum MusicType
        {
            MainMenu,
            Game,
            GameOver
            // Add more as needed
        }
        
        public enum SfxType
        {
            MenuNavigate,
            MenuSelect,
            Jump,
            CollectablePickup
            // Add more as needed
        }
        
        [Header("Music Clips")]
        [SerializeField] private List<MusicTypeClipPair> musicClipsList;

        [Header("SFX Clips")]
        [SerializeField] private List<SfxTypeClipPair> sfxClipsList;
        
        // Internal dictionaries
        private Dictionary<MusicType, (AudioClip clip, float volume)> _musicClips;
        private Dictionary<SfxType, (AudioClip clip, float volume)> _sfxClips;
        
        private AudioClip _currentMusicClip;
        private MusicType _currentMusicType;
        
        [System.Serializable]
        public class MusicTypeClipPair
        {
            public MusicType type;
            public AudioClip clip;
            [Range(0f, 1f)] public float volume = 1;
        }
        
        [System.Serializable]
        public class SfxTypeClipPair
        {
            public SfxType type;
            public AudioClip clip;
            [Range(0f, 2f)] public float volume = 1;
        }
        
        private void Awake()
        {
            // Build dictionaries for fast lookup
            _musicClips = new Dictionary<MusicType, (AudioClip clip, float volume)>();
            foreach (var pair in musicClipsList)
            {
                if (pair.clip != null && !_musicClips.ContainsKey(pair.type))
                    _musicClips.Add(pair.type, (pair.clip, pair.volume));
            }

            _sfxClips = new Dictionary<SfxType, (AudioClip clip, float volume)>();
            foreach (var pair in sfxClipsList)
            {
                if (pair.clip != null && !_sfxClips.ContainsKey(pair.type))
                    _sfxClips.Add(pair.type, (pair.clip, pair.volume));
            }
            
            if (musicSource == null || sfxSource == null)
            {
                Debug.LogError("GameAudioManager: AudioSources not assigned.");
                return;
            }
            
            // Ensure music source loops
            musicSource.loop = true;

            ApplyGlobalVolumes();
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            PlayMusicForCurrentScene();
        }

        private void Start()
        {
            // Play initial music for the current scene
            PlayMusicForCurrentScene();

            // Subscribe to scene changes
            SceneManager.sceneLoaded += OnSceneLoaded;
        }


        private void PlayMusicForCurrentScene()
        {
            var activeScene = SceneManager.GetActiveScene().name;

            if (activeScene == ManagerRoot.Instance.GameSceneManager.GetMainMenuSceneName())
                PlayMusic(MusicType.MainMenu);
            else if (activeScene == ManagerRoot.Instance.GameSceneManager.GetGameOverMenuSceneName())
                PlayMusic(MusicType.GameOver);
            else
                PlayMusic(MusicType.Game); // default for in-game scenes
        }
        
        public void PlayMusic(MusicType type, bool restartIfSame = false)
        {
            if (!_musicClips.TryGetValue(type, out var entry))
            {
                Debug.LogWarning($"MusicType {type} has no clip assigned.");
                return;
            }

            if (_currentMusicClip == entry.clip && musicSource.isPlaying && !restartIfSame)
                return;

            _currentMusicClip = entry.clip;
            _currentMusicType = type;

            musicSource.clip = entry.clip;
            musicSource.volume = entry.volume * musicVolume * masterVolume;
            musicSource.Play();
        }
        
        public void StopMusic()
        {
            musicSource.Stop();
            _currentMusicClip = null;
        }

        public void PauseMusic()
        {
            musicSource.Pause();
        }
        public void ResumeMusic()
        {
            musicSource.UnPause();
        }
        
        private void UpdateMusicVolume()
        {
            if (_currentMusicClip != null && _musicClips.TryGetValue(_currentMusicType, out var entry))
                musicSource.volume = entry.volume * musicVolume * masterVolume;
        }
        
        // Update per-clip music volume live
        public void UpdateMusicClipVolume(MusicType type, float newVolume)
        {
            if (_musicClips.TryGetValue(type, out var entry))
            {
                _musicClips[type] = (entry.clip, Mathf.Clamp01(newVolume));

                // If currently playing, apply immediately
                if (_currentMusicType == type && _currentMusicClip == entry.clip)
                    UpdateMusicVolume();
            }
        }
        
        public void PlaySfx(SfxType type)
        {
            if (!_sfxClips.TryGetValue(type, out var entry))
            {
                Debug.LogWarning($"SFXType {type} has no clip assigned.");
                return;
            }

            sfxSource.PlayOneShot(entry.clip, entry.volume * sfxVolume * masterVolume);
        }
        
        public void UpdateSfxClipVolume(SfxType type, float newVolume)
        {
            if (_sfxClips.TryGetValue(type, out var entry))
                _sfxClips[type] = (entry.clip, Mathf.Clamp01(newVolume));
        }
        
        public void StopAllSfx()
        {
            sfxSource.Stop();
        }

        #region Global Volumes
        
        public void SetMasterVolume(float value)
        {
            masterVolume = Mathf.Clamp01(value);
            ApplyGlobalVolumes();
        }
        
        public void SetMusicVolume(float value)
        {
            musicVolume = Mathf.Clamp01(value);
            if (_currentMusicClip != null)
                UpdateMusicClipVolume(_currentMusicType, _musicClips[_currentMusicType].volume);
        }
        
        public void SetSfxVolume(float value)
        {
            sfxVolume = Mathf.Clamp01(value);
        }
        
        private void ApplyGlobalVolumes()
        {
            // Only affects currently playing music
            UpdateMusicVolume();
        }

        #endregion
        
        
        /// <summary>
        /// Runs when an inspector item is changed
        /// </summary>
        private void OnValidate()
        {
            // Only try to update if dictionaries exist
            if (_musicClips != null && _currentMusicClip != null)
                UpdateMusicVolume();

            if (_musicClips != null && musicClipsList != null)
            {
                foreach (var pair in musicClipsList)
                    UpdateMusicClipVolume(pair.type, pair.volume);
            }

            if (_sfxClips != null && sfxClipsList != null)
            {
                foreach (var pair in sfxClipsList)
                    UpdateSfxClipVolume(pair.type, pair.volume);
            }

            ApplyGlobalVolumes();
        }
    }
}
