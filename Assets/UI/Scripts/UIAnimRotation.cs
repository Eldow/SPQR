using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimRotation : MonoBehaviour {

	public float speed = 0.1f;
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.Rotate(Vector3.forward * speed, Space.World);
	}
}
