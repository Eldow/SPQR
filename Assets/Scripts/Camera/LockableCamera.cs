using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*

    This class manages the camera

*/
public class LockableCamera : MonoBehaviour {

    private bool isLocked;
    private Vector3 offset;
    GameObject player, opponent;

	// Use this for initialization
	void Start () {
        isLocked = true; // For the moment, we keep the camera locked
        offset = new Vector3(0, 3, -2);
    }
	
	// Update is called once per frame
	void LateUpdate () {
        // Lock button pressed : Player look at the opponent
        if (isLocked)
        {
            if (TargetManager.instance.opponents.Count > 0)
            {
                player = TargetManager.instance.player.gameObject;
                opponent = TargetManager.instance.GetNearestOpponent().gameObject;
                transform.parent = player.transform;
                player.transform.LookAt(opponent.transform);
                
            }
        }
        // Lock button not pressed : Player is free
        else
        {
            if (TargetManager.instance.player != null)
            {
                player = TargetManager.instance.player.gameObject;
                transform.parent = player.transform;
            }
        }
	}
}
