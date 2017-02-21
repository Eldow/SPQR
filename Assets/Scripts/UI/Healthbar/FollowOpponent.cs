using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowOpponent : MonoBehaviour {
    public Vector3 offset;
    private RectTransform rect;
    void Start()
    {
        offset = new Vector3(0, 3.5f, 0);
        rect = GetComponent<RectTransform>();
    }
	// Update is called once per frame
	void Update () {
	    if (TargetManager.instance.GetNearestOpponent() == null) return;

        Vector3 pos = TargetManager.instance.GetNearestOpponent().transform.position + offset;  // get the game object position
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(pos);  //convert game object position to VievportPoint

        // set MIN and MAX Anchor values(positions) to the same position (ViewportPoint)
        rect.anchorMin = viewportPoint;
        rect.anchorMax = viewportPoint;
    }
}
