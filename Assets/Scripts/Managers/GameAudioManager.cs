using System.Collections.Generic;
using UnityEngine;
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
            Jump
            // Add more as needed
        }
        
        [Header("Music Clips")]
        [SerializeField] private List<MusicTypeClipPair> musicClipsList;

        [Header("SFX Clips")]
        [SerializeField] private List<SfxTypeClipPair> sfxClipsList;
        
        // Internal dictionaries
        private Dictionary<MusicType, AudioClip> _musicClips;
        private Dictionary<SfxType, AudioClip> _sfxClips;
        
        private AudioClip _currentMusic;
        
        // -------------------------
        // Structs for inspector assignment
        // -------------------------
        [System.Serializable]
        public struct MusicTypeClipPair
        {
            public MusicType type;
            public AudioClip clip;
        }
        
        [System.Serializable]
        public struct SfxTypeClipPair
        {
            public SfxType type;
            public AudioClip clip;
        }
        
        private void Awake()
        {
            // Build dictionaries for fast lookup
            _musicClips = new Dictionary<MusicType, AudioClip>();
            foreach (var pair in musicClipsList)
            {
                if (pair.clip != null && !_musicClips.ContainsKey(pair.type))
                    _musicClips.Add(pair.type, pair.clip);
            }
            
            _sfxClips = new Dictionary<SfxType, AudioClip>();
            foreach (var pair in sfxClipsList)
            {
                if (pair.clip != null && !_sfxClips.ContainsKey(pair.type))
                    _sfxClips.Add(pair.type, pair.clip);
            }
            
            if (musicSource == null || sfxSource == null)
            {
                Debug.LogError("GameAudioManager: AudioSources not assigned.");
                return;
            }
            
            // Ensure music source loops
            musicSource.loop = true;

            ApplyVolumes();
        }

        private void PlayMusic(AudioClip clip, bool restartIfSame = false)
        {
            if (clip == null) return;

            if (_currentMusic == clip && musicSource.isPlaying && !restartIfSame) return;

            _currentMusic = clip;
            musicSource.clip = clip;
            musicSource.Play();
        }
        
        public void PlayMusic(MusicType type, bool restartIfSame = false)
        {
            if (_musicClips.TryGetValue(type, out AudioClip clip))
                PlayMusic(clip, restartIfSame);
            else
                Debug.LogWarning($"MusicType {type} has no clip assigned.");
        }
        
        public void StopMusic()
        {
            musicSource.Stop();
            _currentMusic = null;
        }

        public void PauseMusic()
        {
            musicSource.Pause();
        }
        public void ResumeMusic()
        {
            musicSource.UnPause();
        }

        private void PlaySfx(AudioClip clip)
        {
            if (clip == null)
                return;

            sfxSource.PlayOneShot(clip, sfxVolume * masterVolume);
        }
        
        public void PlaySfx(SfxType type)
        {
            if (_sfxClips.TryGetValue(type, out AudioClip clip))
                PlaySfx(clip);
            else
                Debug.LogWarning($"SFXType {type} has no clip assigned.");
        }
        
        public void StopAllSfx()
        {
            sfxSource.Stop();
        }

        public void SetMasterVolume(float value)
        {
            masterVolume = Mathf.Clamp01(value);
            ApplyVolumes();
        }
        
        public void SetMusicVolume(float value)
        {
            musicVolume = Mathf.Clamp01(value);
            ApplyVolumes();
        }
        
        public void SetSfxVolume(float value)
        {
            sfxVolume = Mathf.Clamp01(value);
            ApplyVolumes();
        }
        
        private void ApplyVolumes()
        {
            if (musicSource != null)
                musicSource.volume = musicVolume * masterVolume;

            if (sfxSource != null)
                sfxSource.volume = sfxVolume * masterVolume;
        }
    }
}
