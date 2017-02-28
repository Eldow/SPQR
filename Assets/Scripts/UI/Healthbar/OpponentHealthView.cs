using UnityEngine;

public class OpponentHealthView : MonoBehaviour
{
    private PlayerController target;
    private float startPosition = 0f;
    private float position;
    public float animationSpeed = 5f;
    private RectTransform rect;

    public void Start()
    {
        rect = GetComponent<RectTransform>();
        target = TargetManager.instance.GetNearestOpponent().GetComponent<PlayerController>();
        startPosition = rect.anchoredPosition.x;
        rect.anchoredPosition = new Vector3(0, rect.anchoredPosition.y);
    }
    public void Update()
    {
        position = startPosition * (1 - target.PlayerHealth.Health / (float)PlayerHealth.MaxHealth);
        //position = startPosition * (1 - PhotonNetwork.playerList[0].GetHealth() / (float)PlayerHealth.MaxHealth);
        rect.anchoredPosition = Vector3.Lerp(rect.anchoredPosition, new Vector3(position, rect.anchoredPosition.y), Time.deltaTime * animationSpeed);
    }
}
