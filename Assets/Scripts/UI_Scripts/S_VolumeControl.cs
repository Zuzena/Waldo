using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class S_VolumeControl : MonoBehaviour
{
    public Slider volumeBGMSlider;
    public static S_AudioManager audioManager;

    private void Start()
    {
        audioManager = S_AudioManager.instance; 

        //Initialize Slider and set volume
        float savedBGMVolume = PlayerPrefs.GetFloat("Volume", 0.5f);
        volumeBGMSlider.value = savedBGMVolume;
        SetVolume(savedBGMVolume);

        //listen for changes
        volumeBGMSlider.onValueChanged.AddListener(SetVolume);

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
}
