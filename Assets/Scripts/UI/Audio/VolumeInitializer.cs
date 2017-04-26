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
	// Use this for initialization
	void Start () {
        Mixer.audioMixer.GetFloat("MasterVolume", out _masterVolume);
        Mixer.audioMixer.GetFloat("MusicVolume", out _musicVolume);
        Mixer.audioMixer.GetFloat("FXVolume", out _fxVolume);
        Mixer.audioMixer.GetFloat("UIVolume", out _uiVolume);

        MasterSlider.value = _masterVolume;
        MusicSlider.value = _musicVolume;
        FXSlider.value = _fxVolume;
        UISlider.value = _uiVolume;
    }

    public void ChangeMasterVolume(float volume)
    {
        Mixer.audioMixer.SetFloat("MasterVolume", volume);
    }

    public void ChangeMusicVolume(float volume)
    {
        Mixer.audioMixer.SetFloat("MusicVolume", volume);
    }

    public void ChangeFXVolume(float volume)
    {
        Mixer.audioMixer.SetFloat("FXVolume", volume);
    }

    public void ChangeUIVolume(float volume)
    {
        Mixer.audioMixer.SetFloat("UIVolume", volume);
    }
}
