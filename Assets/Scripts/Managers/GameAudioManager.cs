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
        
        [Header("Music Clips")]
        [SerializeField] private AudioClip menuMusic;
        [SerializeField] private AudioClip gameMusic;
        [SerializeField] private AudioClip gameOverMusic;
        
        [Header("SFX Clips")]
        [SerializeField] private AudioClip jumpSfx;
        
        private AudioClip _currentMusic;
        
        private void Awake()
        {
            if (musicSource == null || sfxSource == null)
            {
                Debug.LogError("GameAudioManager: AudioSources not assigned.");
                return;
            }
            
            // Ensure music source loops
            musicSource.loop = true;

            ApplyVolumes();
        }
        
        public void PlayMusic(AudioClip clip, bool restartIfSame = false)
        {
            if (clip == null)
                return;

            if (_currentMusic == clip && musicSource.isPlaying && !restartIfSame)
                return;

            _currentMusic = clip;
            musicSource.clip = clip;
            musicSource.Play();
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

        public void PlaySfx(AudioClip clip)
        {
            if (clip == null)
                return;

            sfxSource.PlayOneShot(clip, sfxVolume * masterVolume);
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
