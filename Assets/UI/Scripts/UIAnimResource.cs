using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimResource : MonoBehaviour {

	public int resources;
	public int maxResources;
	
	private float register_0;

	void Start () {
		register_0 = maxResources;
	}
	
	void Update () {
		if (register_0 != resources) {
			GetComponent<SpriteRenderer>().color = new Color(1f,(1f-((float) ((float)resources/(float)maxResources))),(1f-((float) ((float)resources/(float)maxResources))));
			register_0 = resources;
		}
	}
}
