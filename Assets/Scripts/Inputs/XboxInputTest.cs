using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class XboxInputTest : MonoBehaviour {

	public XboxInput xboxInput;

	public float RightStickX,RightStickY;
	public float LeftStickX,LeftStickY;
	public float DPadX, DPadY;

	void Start () {

	}
		
	void Update () {
		
		if (InputManager.attackButton()) {
			Debug.Log (InputManager.moveX ());
		}

	}

}
