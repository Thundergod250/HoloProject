using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Mixer & UI")]
    public AudioMixer mainMixer;
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Audio Source")]
    public AudioSource audioMusicSource;
    public AudioSource audioSFXSource;

    private void Start()
    {
        // Apply the loaded slider values to the actual Mixer groups
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);
    }

    private void LateUpdate()
    {
        if (musicSlider != null && sfxSlider != null)
        {
            SetMusicVolume(musicSlider.value);
            SetSFXVolume(sfxSlider.value);
        }
        else if (musicSlider == null)
        {
            GameObject musicSliderFound = GameObject.Find("Music Slider");
            if (musicSliderFound != null)
            {
                musicSlider = musicSliderFound.GetComponent<Slider>();
                // Apply saved value once found so it doesn't jump to default
                musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
            }
        }
        // 3. If SFX is missing, try to find it
        else if (sfxSlider == null)
        {
            GameObject sfxSliderFound = GameObject.Find("SFX Slider");
            if (sfxSliderFound != null)
            {
                sfxSlider = sfxSliderFound.GetComponent<Slider>();
                sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
            }
        }
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            audioMusicSource.Play();
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }

    // This is the "Play Once" function
    public void PlaySFXOnce(AudioClip clip)
    {
        if (clip != null)
        {
            audioSFXSource.PlayOneShot(clip);
        }
    }

    public void PlaySFXLoop(AudioClip clip)
    {
        if (clip != null)
        {
            audioSFXSource.clip = clip;
            audioSFXSource.Play();
        }
    }

    public void StopSFXSound()
    {
        audioSFXSource.Stop();
        audioSFXSource.clip = null;
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip != null)
        {
            //audioMusicSource.PlayOneShot(clip);

            audioMusicSource.clip = clip;
            audioMusicSource.Play();
        }
    }
    public void SetMusicVolume(float value)
    {
        float dB = Mathf.Log10(Mathf.Max(0.0001f, value)) * 20;
        mainMixer.SetFloat("MusicVolume", dB);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        float dB = Mathf.Log10(Mathf.Max(0.0001f, value)) * 20;
        mainMixer.SetFloat("SFXVolume", dB);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }
}
