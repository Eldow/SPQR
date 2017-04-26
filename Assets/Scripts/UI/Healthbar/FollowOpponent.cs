using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowOpponent : MonoBehaviour
{
	
	public Vector3 offset;
	private RectTransform rect;
	private Transform opponent;
	private PlayerController playerController;

	void OnEnable ()
	{
		offset = new Vector3 (0, 3.5f, 0);
		rect = GetComponent<RectTransform> ();
		playerController = GameObject.FindGameObjectWithTag (PlayerController.Player).GetComponent<PlayerController> ();
		opponent = playerController.TargetManager.currentTarget.transform;
	
	}

	void Update ()
	{
		if (opponent == null || playerController==null) {
			this.gameObject.SetActive (false);
			return;
		}
        if (!FightCamera.FightingCamera.enabled) return;
		Vector3 pos = opponent.position + offset;  // get the game object position
		Vector3 viewportPoint = Camera.main.WorldToViewportPoint (pos);  //convert game object position to VievportPoint

		// set MIN and MAX Anchor values(positions) to the same position (ViewportPoint)
		rect.anchorMin = viewportPoint;
		rect.anchorMax = viewportPoint;
	}
}

 