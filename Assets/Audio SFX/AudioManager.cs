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
    public AudioSource audioSource;

    private void Awake()
    {
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

        // Load saved volumes
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
    }

    // This is the "Play Once" function
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
    public void SetMusicVolume(float value)
    {
        float dB = Mathf.Log10(Mathf.Max(0.0001f, value)) * 20;
        mainMixer.SetFloat("MusicVol", dB);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        float dB = Mathf.Log10(Mathf.Max(0.0001f, value)) * 20;
        mainMixer.SetFloat("SFXVol", dB);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }
}
