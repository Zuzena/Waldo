using UnityEngine;

public class S_AudioManager : MonoBehaviour
{
    //Get the object holding this script
    public static S_AudioManager instance {  get; private set; }

    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public AudioClip[] musicTracks;
    public AudioClip[] sfxSounds;
    private int musicIndex = 0;

    //on awake, make sure this is the only one and we're loading in right
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


    }

    public void SetBGMVolume(float volume)
    {
        if (bgmSource != null)
        {
            bgmSource.volume = volume;
        }
        else return; 
    }

    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null)
        {
            sfxSource.volume = volume;
            Debug.Log("the volume has changed"); 
        }
        else return; 
    }

    public void SwapTracks()
    {
        musicIndex += 1;
        if (musicTracks == null) { return; }
        if (musicIndex > 2)
        {
            musicIndex = 0;
            bgmSource.clip = musicTracks[musicIndex];
            bgmSource.Play();
        }
        else
        {
            bgmSource.clip = musicTracks[musicIndex];
            bgmSource.Play();
        }

    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}
