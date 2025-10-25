using UnityEngine;

public class S_AudioManager : MonoBehaviour
{
    //Get the object holding this script
    public static S_AudioManager instance {  get; private set; }

    public AudioSource bgmSource; 

    //on awake, make sure this is the only one
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
    }
}
