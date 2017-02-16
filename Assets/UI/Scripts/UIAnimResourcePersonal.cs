using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimResourcePersonal : MonoBehaviour {

	public int resources;
	public int maxResources;
	
	private float register_0;

	void Start () {
		register_0 = maxResources;
	}
	
	void Update () {
		if (register_0 != resources) {
			transform.Translate(Vector3.up * ((float)(resources - register_0))/((float)maxResources)*150f, Space.World);
			register_0 = resources;
		}
	}
}
