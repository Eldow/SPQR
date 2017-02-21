using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveHit : MonoBehaviour
{
	//Can handle own collider
	void OnCollisionEnter (Collision other)
	{
		//Use for different players
		if (!other.transform.tag.Equals (TargetManager.instance.player.GetComponent<PlayerController> ().tagName) 
			&& other.transform.name.Equals ("Robot:SwordRight") || other.transform.name.Equals ("Robot:SwordLeft")) {
			Debug.Log ("Player got hit in :" + other.contacts [0].thisCollider.name);
			//TargetManager.instance.GetNearestOpponent ().GetComponent<PlayerController> ().TakeDamage (10);
		} 
	}
}
