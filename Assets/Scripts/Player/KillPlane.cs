using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlane : MonoBehaviour
{
	//Can handle own collider
	void OnTriggerEnter (Collider other)
	{
        if (other.GetComponentInParent<PlayerController>() == null) return;
        other.GetComponentInParent<PlayerController>().PlayerHealth.Health = 0;
	}
}
