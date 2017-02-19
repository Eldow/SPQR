using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
	public static float moveX ()
	{
		float r = 0f;
		r += Input.GetAxis ("JoystickMove_Horizontal");
		r += Input.GetAxis ("KeyboardMove_Horizontal");
		return Mathf.Clamp (r, -1.0f, 1.0f);
	}

	public static float moveY ()
	{
		float r = 0f;
		r += Input.GetAxis ("JoystickMove_Vertical");
		r += Input.GetAxis ("KeyboardMove_Vertical");
		return Mathf.Clamp (r, -1.0f, 1.0f);
	}

	public static bool attackButton ()
	{
		return Input.GetButtonDown ("AttackButton");
	}

	public static bool runButton ()
	{
		return Input.GetButtonDown ("RunButton");
	}

	public static bool blockButton ()
	{
		return Input.GetButtonDown ("BlockButton");
	}

	public static bool cameraButton ()
	{
		return Input.GetButtonDown ("CameraButton");
	}


}
