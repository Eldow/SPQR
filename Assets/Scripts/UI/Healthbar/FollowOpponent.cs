using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowOpponent : MonoBehaviour
{
	
	public Vector3 offset;
	private RectTransform rect;
	private Transform opponent;

	void OnEnable ()
	{
		offset = new Vector3 (0, 3.5f, 0);
		rect = GetComponent<RectTransform> ();
		opponent = GameObject.FindGameObjectWithTag (PlayerController.Player).GetComponent<TargetManager> ().currentTarget.transform;
	
	}

	void Update ()
	{
		if (opponent == null) {
			this.gameObject.SetActive (false);
			return;
		}
		Vector3 pos = opponent.position + offset;  // get the game object position
		Vector3 viewportPoint = Camera.main.WorldToViewportPoint (pos);  //convert game object position to VievportPoint

		// set MIN and MAX Anchor values(positions) to the same position (ViewportPoint)
		rect.anchorMin = viewportPoint;
		rect.anchorMax = viewportPoint;
	}
}

