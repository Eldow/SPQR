using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlane : MonoBehaviour
{
	//Can handle own collider
	void OnTriggerEnter (Collider other)
	{
    // Debug.Log("KILL PLANE TRIGGERED");
		PlayerController pc = other.GetComponentInParent<PlayerController>();
		if (pc == null || !pc.photonView.isMine) return;
    pc.PlayerHealth.Health = 0;
	}
}
