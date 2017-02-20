using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveHit : MonoBehaviour
{
	//Can handle own collider
	void OnCollisionEnter (Collision other)
	{
		//Use for different players
		if (other.transform.name.Equals ("Robot:SwordRight") || other.transform.name.Equals ("Robot:SwordLeft")) {
			Debug.Log ("Player got hit in :" + other.contacts [0].thisCollider.name);
            TargetManager.instance.GetNearestOpponent().GetComponent<PlayerController>().TakeDamage(10);
            // TODO fix this <3


            /*PlayerController p = TargetManager.instance.GetNearestOpponent().GetComponent<PlayerController>();
            TargetManager.instance.GetNearestOpponent().GetComponent<PlayerController>().automaton.enabled = true;
            TargetManager.instance.GetNearestOpponent().GetComponent<PlayerController>().automaton.StateMachine.SetState(new RobotHitstunState(200));*/
        }
	}
}
