using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudioBehaviour : MonoBehaviour {

    public AudioClip[] ClickClips;
    public AudioClip[] SlideClips;

    private AudioSource _audioSource;

    private float _lowPitch = .90f;
    private float _highPitch = 1.0f;

    void Start()
    {
		if (GameObject.FindGameObjectsWithTag ("Audio").Length > 1)
			Destroy (this.gameObject);
		
        DontDestroyOnLoad(this);
    }

    private void PlayRandomClipInArray(AudioClip[] array)
    {
        _audioSource = GameObject.Find("UIAudio").GetComponent<AudioSource>();
        _audioSource.pitch = Random.Range(_lowPitch, _highPitch);
        int _randomIndex;
        if (array.Length == 1)
        {
            _audioSource.PlayOneShot(array[0]);
        }
        else if (array.Length > 0)
        {
            _randomIndex = Random.Range(0, array.Length);
            _audioSource.PlayOneShot(array[_randomIndex]);
        }
    }

    public void PlayAudioClick()
    {
        PlayRandomClipInArray(ClickClips);
    }

    public void PlaySlideClip()
    {
        //PlayRandomClipInArray(SlideClips);
    }
}
