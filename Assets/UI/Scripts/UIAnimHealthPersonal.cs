using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimHealthPersonal : MonoBehaviour {

	public int health;
	public int maxHealth;
	
	private float register_0;

	void Start () {
		register_0 = maxHealth;
	}
	
	void Update () {
		if (register_0 != health) {
			transform.Rotate(Vector3.forward * ( ((float)(health - register_0))/((float)maxHealth)*90f ), Space.World);
			register_0 = health;
		}
	}
}