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

	public static float cameraX()
	{
		float r = 0f;
		r += Input.GetAxis ("JoystickCamera_Horizontal");
		r += Input.GetAxis ("MouseCamera_Horizontal");
		return Mathf.Clamp (r, -1.0f, 1.0f);
	}

	public static float cameraY()
	{
		float r = 0f;
		r += Input.GetAxis ("JoystickCamera_Vertical");
		r += Input.GetAxis ("MouseCamera_Vertical");
		return Mathf.Clamp (r, -1.0f, 1.0f);
	}

	public static bool attackButton ()
	{
		return Input.GetButtonDown ("AttackButton");
	}

	public static bool runButton ()
	{
		return Input.GetButton ("RunButton") || Mathf.Abs( Input.GetAxis ("RunJoystick")) > 0.3f;
	}

	public static bool blockButton ()
	{
		return Input.GetButton ("BlockButton");
	}

	public static bool cameraButton ()
	{
		return Input.GetButton ("CameraButton");
	}

	public static bool cameraButtonDown ()
	{
		return Input.GetButtonDown ("CameraButton");
	}


}
