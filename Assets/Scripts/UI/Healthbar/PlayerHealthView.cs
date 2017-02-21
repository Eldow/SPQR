using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthView : MonoBehaviour
{
    private PlayerController target;
    private float startRotation = 136f;
    private float rotation;
    public float animationSpeed = 5f;
    private RectTransform rect;

    public void Start()
    {
        rect = GetComponent<RectTransform>();
        target = TargetManager.instance.player.GetComponent<PlayerController>();
        startRotation = rect.eulerAngles.z;
        rect.eulerAngles = new Vector3(rect.eulerAngles.x, rect.eulerAngles.y, 0);
    }
    public void FixedUpdate()
    {
        rotation = startRotation * (1.0f - (PhotonNetwork.player.GetHealth() / (float)PlayerHealth.MaxHealth));
        rect.eulerAngles = Vector3.Lerp(rect.eulerAngles, new Vector3(rect.eulerAngles.x, rect.eulerAngles.y, rotation), Time.deltaTime * animationSpeed);
    }
}
