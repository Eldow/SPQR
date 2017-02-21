using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerView : MonoBehaviour
{
    private PlayerController target;
    private float startPosition = 0f;
    private float position;
    public float animationSpeed = 5f;
    private RectTransform rect;

    public void Start()
    {
        rect = GetComponent<RectTransform>();
        target = TargetManager.instance.player.GetComponent<PlayerController>();
        startPosition = rect.anchoredPosition.y;
    }
    public void Update()
    {
        position = startPosition * (1 - target.PowerManager.CurrentPower / (float) PowerManager.MaxPower);
        rect.anchoredPosition = Vector3.Lerp(rect.anchoredPosition, new Vector3(rect.anchoredPosition.x, position), Time.deltaTime * animationSpeed);
    }
}
