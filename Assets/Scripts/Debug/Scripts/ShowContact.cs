using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowContact : MonoBehaviour
{

	public GameObject hitPointPrefab;
	public float timeToDestroy = 5.0f;
	// Public static intance to the manager
	public static ShowContact ShowContactInstance = null;

	// Stores a static instance of this target manager to access it from anywhere at anytime
	void Start ()
	{
		if (ShowContactInstance == null) {
			ShowContactInstance = this;
		}
	}

	public void showContactPoint (Collision other)
	{
		GameObject go = Instantiate (hitPointPrefab, other.contacts [0].otherCollider.transform) as GameObject; 
		go.transform.position = other.contacts [0].point;
		Destroy (go, timeToDestroy);

	}

}
