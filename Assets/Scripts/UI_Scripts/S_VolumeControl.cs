using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class S_VolumeControl : MonoBehaviour
{
    public Slider volumeBGMSlider;
    public Slider volumeSFXSlider; 
    public static S_AudioManager audioManager;

    private void Start()
    {
        audioManager = S_AudioManager.instance; 

        //Initialize Slider and set volume
        float savedBGMVolume = PlayerPrefs.GetFloat("Volume", 0.5f);
        volumeBGMSlider.value = savedBGMVolume;
        SetVolume(savedBGMVolume);
        float savedSFXVolume = PlayerPrefs.GetFloat("Volume", 0.5f); 
        volumeSFXSlider.value = savedSFXVolume;
        SFXVolume(savedSFXVolume); 

        //listen for changes
        volumeBGMSlider.onValueChanged.AddListener(SetVolume);
        volumeSFXSlider.onValueChanged.AddListener(SFXVolume);
    }

    //set and save the volume, actually
    public void SetVolume(float volume)
    {
        if (audioManager != null) {
            audioManager.SetBGMVolume(volume);
            PlayerPrefs.SetFloat("Volume", volume);
            PlayerPrefs.Save();
        }
    }

    public void SFXVolume(float volume)
    {
        if (audioManager != null)
        {
            audioManager.SetSFXVolume(volume);
            PlayerPrefs.SetFloat("Volume", volume); 
            PlayerPrefs.Save();
        }
    }
}
