using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicSource;                 //Drag a reference to the audio source which will play the music.
    public AudioSource uiSource;
    public static SoundManager instance = null;     //Allows other scripts to call functions from SoundManager.             
    public float lowPitchRange = .90f;              //The lowest a sound effect will be randomly pitched.
    public float highPitchRange = 1.00f;            //The highest a sound effect will be randomly pitched.

    public AudioClip LobbyMusic;
    public AudioClip GameMusic;

    void Awake()
    {
        //Check if there is already an instance of SoundManager
        if (instance == null)
            //if not, set it to this.
            instance = this;
        //If instance already exists:
        else if (instance != this)
            //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
            Destroy(gameObject);

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }

    public void PlayClick()
    {
        uiSource.pitch = Random.Range(lowPitchRange, highPitchRange);
        uiSource.Play();
    }

    public void PlayLobbyMusic()
    {
        musicSource.clip = LobbyMusic;
        musicSource.Play();
    }

    public void PlayGameMusic()
    {
        musicSource.clip = GameMusic;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}
