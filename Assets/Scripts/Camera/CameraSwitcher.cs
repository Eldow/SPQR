using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSwitcher : MonoBehaviour {

    public static CameraSwitcher instance;
    public Image BlackScreen;

    void Start()
    {
        instance = this;
    }

	public void SwitchCamera()
    {
        BlackScreen.enabled = true;
        StartCoroutine(SwitchAfterDelay(2f));
    }

    IEnumerator SwitchAfterDelay(float delay)
    {
        FadeToBlack(delay / 2);
        yield return new WaitForSeconds(delay/2);
        SpectateCamera.SpectatorCamera.enabled = !SpectateCamera.SpectatorCamera.enabled;
        FightCamera.FightingCamera.enabled = !FightCamera.FightingCamera.enabled;
        FadeFromBlack(delay / 2);
        yield return new WaitForSeconds(delay / 2);
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
