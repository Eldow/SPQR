using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlane : MonoBehaviour
{
	//Can handle own collider
	void OnTriggerEnter (Collider other)
	{
			Debug.Log("KILL PLANE TRIGGERED");
			Debug.Log(other);
	}
}
