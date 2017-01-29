using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XboxInputTest : MonoBehaviour {

	public int id;
	public XboxInput xboxInput;

	float RightStickX,RightStickY;
	float LeftStickX,LeftStickY;
	float DPadX, DPadY;

	void Start () {
		xboxInput = new XboxInput (id);
	}
		
	void Update () {
		if(xboxInput.LT())
			print ("Controller "+xboxInput.id + " : LT PRESSED");

		if(xboxInput.RT())
			print ("Controller "+xboxInput.id + " : RT PRESSED");

		RightStickX = xboxInput.getRightStickX ();
		RightStickY = xboxInput.getRightStickY ();

		LeftStickX = xboxInput.getLeftStickX ();
		LeftStickY = xboxInput.getLeftStickY ();

		DPadX = xboxInput.getDPadX ();
		DPadY = xboxInput.getDPadY ();

		//print (DPadX + " " + DPadY);

		foreach (KeyValuePair<string,KeyCode> current in xboxInput.allButtons) {
			if (Input.GetKeyDown (current.Value)) {
				print ("Controller "+xboxInput.id + " : "+ current.Key);

			}	
		}
	}
}
