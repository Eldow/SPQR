using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimHealth : MonoBehaviour {

	public int health;
	public int maxHealth;
	
	private float register_0;

	void Start () {
		register_0 = maxHealth;
	}
	
	void Update () {
		if (register_0 != health) {
			transform.localScale = new Vector3((float) ((float)health/(float)maxHealth),1f,1f);
			register_0 = health;
		}
	}
}
