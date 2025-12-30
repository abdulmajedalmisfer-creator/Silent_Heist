using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    public AudioMixer mainMixer;
    public Slider sfxSlider;
    public Slider MusicSlider;

    void Start()
    {
        if (sfxSlider != null) sfxSlider.minValue = 0.0001f;
        if (MusicSlider != null) MusicSlider.minValue = 0.0001f;

 
        float sfxDb;
        if (mainMixer.GetFloat("SFXVolume", out sfxDb))
            sfxSlider.value = Mathf.Pow(10f, sfxDb / 20f);

        float musicDb;
        if (mainMixer.GetFloat("MusicVolume", out musicDb))
            MusicSlider.value = Mathf.Pow(10f, musicDb / 20f);

  
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        MusicSlider.onValueChanged.AddListener(SetMusicVolume);
    }

    public void SetSFXVolume(float sliderValue)
    {
        float db = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;
        mainMixer.SetFloat("SFXVolume", db);
    }

    public void SetMusicVolume(float sliderValue)
    {
        float db = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;
        mainMixer.SetFloat("MusicVolume", db);
    }
}
