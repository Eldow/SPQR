using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class XboxInput  {

	private float triggerMagnitudeMin = -0.2f;

	public int id;
	public Dictionary<string, KeyCode> allButtons;

	/*WARNING : LT and RT are considered as sticks since they can have different magnitude values*/
	/*Basic buttons*/
	public KeyCode A, B, X, Y;
	public KeyCode Start, Select;
	/*Left and Right Bumber*/
	public KeyCode LB, RB;
	/*Left and Right Stick Button*/
	public KeyCode LJB, RJB;

	public XboxInput(int id){
		this.id = id;
		//See http://wiki.unity3d.com/index.php?title=Xbox360Controller for button number
		A = (KeyCode)System.Enum.Parse (typeof(KeyCode), "Joystick" + id + "Button0");
		B = (KeyCode)System.Enum.Parse (typeof(KeyCode), "Joystick" + id + "Button1");
		X = (KeyCode)System.Enum.Parse (typeof(KeyCode), "Joystick" + id + "Button2");
		Y = (KeyCode)System.Enum.Parse (typeof(KeyCode), "Joystick" + id + "Button3");
		LB = (KeyCode)System.Enum.Parse (typeof(KeyCode), "Joystick" + id + "Button4");
		RB = (KeyCode)System.Enum.Parse (typeof(KeyCode), "Joystick" + id + "Button5");
		Select = (KeyCode)System.Enum.Parse (typeof(KeyCode), "Joystick" + id + "Button6");
		Start = (KeyCode)System.Enum.Parse (typeof(KeyCode), "Joystick" + id + "Button7");
		LJB = (KeyCode)System.Enum.Parse (typeof(KeyCode), "Joystick" + id + "Button8");
		RJB = (KeyCode)System.Enum.Parse (typeof(KeyCode), "Joystick" + id + "Button9");

		allButtons = new Dictionary<string, KeyCode> ();
		allButtons.Add("A",A);
		allButtons.Add("B", B);
		allButtons.Add("X", X);
		allButtons.Add("Y", Y);
		allButtons.Add("LB", LB);
		allButtons.Add("RB", RB);
		allButtons.Add("SELECT", Select);
		allButtons.Add("START", Start);
		allButtons.Add("LJB", LJB);
		allButtons.Add("RJB", RJB);
	}

	public bool LT(){
		return Input.GetAxis ("LT_"+ id) < triggerMagnitudeMin;
	}

	public bool RT(){
		return Input.GetAxis ("RT_"+ id) < triggerMagnitudeMin;
	}

	/*STICKS*/
	public float getLeftStickX(){
		return Input.GetAxis ("LX_" + id);
	}

	public float getLeftStickY(){
		return Input.GetAxis ("LY_" + id);
	}


	public float getRightStickX(){
		return Input.GetAxis ("RX_" + id);
	}

	public float getRightStickY(){
		return Input.GetAxis ("RY_" + id);
	}
		

	public float getDPadX(){
		return Input.GetAxis ("DPADX_" + id);
	}

	public float getDPadY(){
		return Input.GetAxis ("DPADY_" + id);
	}

}

