﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSwitcher : MonoBehaviour {

    public static CameraSwitcher instance;
    public Image BlackScreen;
    public bool Switching;

    void Start()
    {
        instance = this;
        Switching = false;
    }

	public void SwitchCamera()
    {
        Switching = true;
        StartCoroutine(SwitchAfterDelay(2f));
    }

    IEnumerator SwitchAfterDelay(float delay)
    {
        yield return new WaitForSeconds(1f);
        BlackScreen.enabled = true;
        FadeToBlack(delay / 2);
        yield return new WaitForSeconds(delay/2);
        SpectateCamera.SpectatorCamera.enabled = true;
        FightCamera.FightingCamera.enabled = false;
        FadeFromBlack(delay / 2);
        yield return new WaitForSeconds(delay / 2);
        BlackScreen.enabled = false;
    }

    void FadeToBlack(float time)
    {
        BlackScreen.color = Color.black;
        BlackScreen.canvasRenderer.SetAlpha(0.0f);
        BlackScreen.CrossFadeAlpha(1.0f, time, false);
    }

    void FadeFromBlack(float time)
    {
        BlackScreen.color = Color.black;
        BlackScreen.canvasRenderer.SetAlpha(1.0f);
        BlackScreen.CrossFadeAlpha(0.0f, time, false);
    }
}
