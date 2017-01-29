using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XboxInputTest : MonoBehaviour {

	public XboxInput xboxInput;

	public float RightStickX,RightStickY;
	public float LeftStickX,LeftStickY;
	public float DPadX, DPadY;

	void Start () {
		xboxInput = new XboxInput (1);
	}
		
	void Update () {
		
		if(xboxInput.LT())
			print ("Controller "+xboxInput.getID() + " : LT PRESSED");

		if(xboxInput.RT())
			print ("Controller "+xboxInput.getID() + " : RT PRESSED");

		RightStickX = xboxInput.getRightStickX ();
		RightStickY = xboxInput.getRightStickY ();

		LeftStickX = xboxInput.getLeftStickX ();
		LeftStickY = xboxInput.getLeftStickY ();

		DPadX = xboxInput.getDPadX ();
		DPadY = xboxInput.getDPadY ();

		//print (LeftStickX + " " + LeftStickY);
		foreach (KeyValuePair<string,KeyCode> current in xboxInput.getAllButtons()) {
			if (Input.GetKeyDown (current.Value)) {
				print ("Controller "+xboxInput.getID() + " : "+ current.Key);

			}	
		}
	}
}
