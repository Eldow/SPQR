using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeInitializer : MonoBehaviour {

    public AudioMixerGroup Mixer;
    public Slider MasterSlider;
    public Slider MusicSlider;
    public Slider FXSlider;
    public Slider UISlider;

    private float _masterVolume;
    private float _musicVolume;
    private float _fxVolume;
    private float _uiVolume;

    private const string _masterIdentifier = "MasterVolume";
    private const string _musicIdentifier = "MusicVolume";
    private const string _fxIdentifier = "FXVolume";
    private const string _uiIdentifier = "UIVolume";

    // Use this for initialization
    void Start () {
        Mixer.audioMixer.GetFloat(_masterIdentifier, out _masterVolume);
        Mixer.audioMixer.GetFloat(_musicIdentifier, out _musicVolume);
        Mixer.audioMixer.GetFloat(_fxIdentifier, out _fxVolume);
        Mixer.audioMixer.GetFloat(_uiIdentifier, out _uiVolume);

        // If keys are present in playerprefs, retrieves the user preferences
        // Otherwise, keeps the original value
        _masterVolume = PlayerPrefs.GetFloat(_masterIdentifier, _masterVolume);
        _musicVolume = PlayerPrefs.GetFloat(_musicIdentifier, _musicVolume);
        _fxVolume = PlayerPrefs.GetFloat(_fxIdentifier, _fxVolume);
        _uiVolume =  PlayerPrefs.GetFloat(_uiIdentifier, _uiVolume);

        MasterSlider.value = _masterVolume;
        MusicSlider.value = _musicVolume;
        FXSlider.value = _fxVolume;
        UISlider.value = _uiVolume;
    }

    /* Apply modifications and save them in preferences for future uses */

    public void ChangeMasterVolume(float volume)
    {
        Mixer.audioMixer.SetFloat(_masterIdentifier, volume);
        PlayerPrefs.SetFloat(_masterIdentifier, volume);
    }

    public void ChangeMusicVolume(float volume)
    {
        Mixer.audioMixer.SetFloat(_musicIdentifier, volume);
        PlayerPrefs.SetFloat(_musicIdentifier, volume);
    }

    public void ChangeFXVolume(float volume)
    {
        Mixer.audioMixer.SetFloat(_fxIdentifier, volume);
        PlayerPrefs.SetFloat(_fxIdentifier, volume);
    }

    public void ChangeUIVolume(float volume)
    {
        Mixer.audioMixer.SetFloat(_uiIdentifier, volume);
        PlayerPrefs.SetFloat(_uiIdentifier, volume);
    }
}
