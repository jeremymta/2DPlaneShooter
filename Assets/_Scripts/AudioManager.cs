using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource musicSource;
    public AudioSource effectsSource;

    public AudioClip backgroundMusic;
    public AudioClip playerHitSound;
    public AudioClip shootSound;
    public AudioClip gameOverSound;

    public Slider musicSlider;
    private float musicVolume;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // add AudioSource components
        if (musicSource == null)
            musicSource = gameObject.AddComponent<AudioSource>();

        if (effectsSource == null)
            effectsSource = gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        PlayBackgroundMusic();
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        musicSlider.value = musicVolume;
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        SetMusicVolume(musicVolume);
    }
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        musicSource.volume = musicVolume;

        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.Save();
    }

    public void PlayBackgroundMusic()
    {
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayPlayerHitSound()
    {
        effectsSource.PlayOneShot(playerHitSound);
    }

    public void PlayShootSound()
    {
        effectsSource.PlayOneShot(shootSound);
    }

    public void PlayGameOverSound()
    {
        musicSource.Stop();
        musicSource.loop = false;
        musicSource.clip = gameOverSound;
        musicSource.Play();
    }

    public void StopBackgroundMusic()
    {
        musicSource.Stop();
    }

    public void ResetMusic()
    {
        StopAllCoroutines();
        StopAllSounds();
        PlayBackgroundMusic();
    }

    public void StopAllSounds()
    {
        musicSource.Stop();
        effectsSource.Stop();
    }
}