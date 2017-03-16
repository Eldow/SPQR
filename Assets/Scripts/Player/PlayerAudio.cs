using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : Photon.MonoBehaviour {

    private AudioSource _audioSource;
    private int _randomIndex;
    private string _methodName;
    private PlayerController _playerController;

    public AudioClip[] AttackClips;
    public AudioClip[] LightHitClips;
    public AudioClip[] MediumHitClips;
    public AudioClip[] StrongHitClips;
    public AudioClip[] OverloadClips;
    public AudioClip[] SpeedUpClips;
    public AudioClip[] SlowDownClips;
    public AudioClip[] BrakeClips;
    public AudioClip[] BlockClips;
    public AudioClip[] StunClips;
    public AudioClip[] StunHitClips;
    public AudioClip[] DestructionClips;
    public AudioClip WinClip;
    public AudioClip LoseClip;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _playerController = GetComponent<PlayerController>();
    }

    private void SendToOthers()
    {
        if (_playerController.ID != GameManager.Instance.LocalPlayer.ID) return;
        _playerController.UpdateAudioToOthers(_methodName);
    }

    private void PlayRandomClipInArray(AudioClip[] array)
    {
        SendToOthers();
        if(array.Length == 1)
        {
            _audioSource.PlayOneShot(array[0]);
        }
        else if (array.Length > 0)
        {
            _randomIndex = Random.Range(0, array.Length);
            _audioSource.PlayOneShot(array[_randomIndex]);
        }
    }

    public void Attack()
    {
        _methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
        PlayRandomClipInArray(AttackClips);
    }

    public void Whirlwind()
    {
        _methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
        StartCoroutine(WhirlwindAttack());
    }

    IEnumerator WhirlwindAttack()
    {
        SendToOthers();
        _audioSource.PlayOneShot(AttackClips[0]);
        yield return new WaitForSeconds(0.1f);
        _audioSource.PlayOneShot(AttackClips[1]);
        yield return new WaitForSeconds(0.1f);
        _audioSource.PlayOneShot(AttackClips[2]);
        yield return new WaitForSeconds(0.1f);
    }

    public void Hit(int force = 1)
    {
        _methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
        if (force == 1)
        {
            PlayRandomClipInArray(LightHitClips);
        }
        else if (force == 2)
        {
            PlayRandomClipInArray(MediumHitClips);
        }
        else if (force == 3)
        {
            PlayRandomClipInArray(StrongHitClips);
        }
    }

    public void StunHit()
    {
        _methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
        PlayRandomClipInArray(StunHitClips);
    }

    public void Overload()
    {
        _methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
        PlayRandomClipInArray(OverloadClips);
    }

    public void SpeedUp()
    {
        _methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
        PlayRandomClipInArray(SpeedUpClips);
    }

    public void SlowDown()
    {
        _methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
        PlayRandomClipInArray(SlowDownClips);
    }

    public void Dash()
    {
        _methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
        PlayRandomClipInArray(SpeedUpClips);
    }

    public void Brake()
    {
        _methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
        PlayRandomClipInArray(BrakeClips);
    }

    public void Block()
    {
        _methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
        PlayRandomClipInArray(BlockClips);
    }

    public void Stun()
    {
        _methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
        PlayRandomClipInArray(StunClips);
    }

    public void Destruction()
    {
        _methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
        PlayRandomClipInArray(DestructionClips);
    }

    public void Win()
    {
        StartCoroutine(PlayWin());
    }

    IEnumerator PlayWin()
    {
        yield return new WaitForSeconds(1f);
        _audioSource.PlayOneShot(WinClip);
    }

    public void Lose()
    {
        StartCoroutine(PlayLose());
    }

    IEnumerator PlayLose()
    {
        yield return new WaitForSeconds(1f);
        _audioSource.PlayOneShot(LoseClip);
    }

}
