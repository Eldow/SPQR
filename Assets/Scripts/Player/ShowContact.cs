using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowContact : MonoBehaviour
{

	public GameObject hitPointPrefab;


	void OnCollisionEnter (Collision other)
	{
		if (other.transform.tag.Equals (PlayerController.Opponent)) {
			GameObject go = Instantiate (hitPointPrefab, other.transform) as GameObject; 
			go.transform.position = other.contacts [0].point;
			Destroy (go, 5.0f);
		}
	}

}
