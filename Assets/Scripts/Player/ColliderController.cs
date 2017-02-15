using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderController : MonoBehaviour
{

	public GameObject hitPointPrefab;

	/*void OnTriggerEnter (Collider test)
	{
		print ("OnTriggerEnter : " + test.name); 
	}*/

	void OnCollisionEnter (Collision other)
	{
		//print ("Points colliding: " + other.contacts.Length);
		//print ("First point that collided: " + other.contacts [0].point);
		GameObject go = Instantiate (hitPointPrefab, other.transform) as GameObject; 
		go.transform.position = other.contacts [0].point;
		Destroy (go, 5.0f);
	}

}
