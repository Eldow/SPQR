using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightCamera : MonoBehaviour {

    public static Camera FightingCamera;
	// Use this for initialization
	void Start () {
        FightingCamera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
