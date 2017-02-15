using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class XboxInput
{

	private float triggerMagnitudeMin = 0.2f;

	private int id;
	private Dictionary<string, KeyCode> allButtons;

	/*WARNING : LT and RT are considered as sticks since they can have different magnitude values*/
	/*Basic buttons*/
	public KeyCode A, B, X, Y;
	public KeyCode Start, Select;
	/*Left and Right Bumber*/
	public KeyCode LB, RB;
	/*Left and Right Stick Button*/
	public KeyCode LJB, RJB;

	#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
	public KeyCode Dpad_UP,Dpad_DOWN,Dpad_LEFT,Dpad_RIGHT;
	#endif
	public XboxInput (int id)
	{
		this.id = id;
		initialiseButtons ();
		allButtons = new Dictionary<string, KeyCode> ();
		allButtons.Add ("A", A);
		allButtons.Add ("B", B);
		allButtons.Add ("X", X);
		allButtons.Add ("Y", Y);
		allButtons.Add ("LB", LB);
		allButtons.Add ("RB", RB);
		allButtons.Add ("SELECT", Select);
		allButtons.Add ("START", Start);
		allButtons.Add ("LJB", LJB);
		allButtons.Add ("RJB", RJB);
	}

	private void initialiseButtons ()
	{
		#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

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

		#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
		//See http://wiki.unity3d.com/index.php?title=Xbox360Controller for button number
		A = (KeyCode)System.Enum.Parse (typeof(KeyCode), "Joystick" + id + "Button16");
		B = (KeyCode)System.Enum.Parse (typeof(KeyCode), "Joystick" + id + "Button17");
		X = (KeyCode)System.Enum.Parse (typeof(KeyCode), "Joystick" + id + "Button18");
		Y = (KeyCode)System.Enum.Parse (typeof(KeyCode), "Joystick" + id + "Button19");
		LB = (KeyCode)System.Enum.Parse (typeof(KeyCode), "Joystick" + id + "Button13");
		RB = (KeyCode)System.Enum.Parse (typeof(KeyCode), "Joystick" + id + "Button14");
		Select = (KeyCode)System.Enum.Parse (typeof(KeyCode), "Joystick" + id + "Button10");
		Start = (KeyCode)System.Enum.Parse (typeof(KeyCode), "Joystick" + id + "Button9");
		LJB = (KeyCode)System.Enum.Parse (typeof(KeyCode), "Joystick" + id + "Button11");
		RJB = (KeyCode)System.Enum.Parse (typeof(KeyCode), "Joystick" + id + "Button12");

		Dpad_UP = (KeyCode)System.Enum.Parse (typeof(KeyCode), "Joystick" + id + "Button5");
		Dpad_DOWN = (KeyCode)System.Enum.Parse (typeof(KeyCode), "Joystick" + id + "Button6");
		Dpad_LEFT = (KeyCode)System.Enum.Parse (typeof(KeyCode), "Joystick" + id + "Button7");
		Dpad_RIGHT = (KeyCode)System.Enum.Parse (typeof(KeyCode), "Joystick" + id + "Button8");

		#else
		Debug.Log("CONTROLS AREN'T DEFINED FOR LINUX");

		#endif
	}

	public bool LT ()
	{
		#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
		return Input.GetAxis ("LT_" + id) < triggerMagnitudeMin;
		#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
		return Input.GetAxis ("MAC_LT_" + id) < triggerMagnitudeMin;
		#else
		Debug.Log("CONTROLS AREN'T DEFINED FOR LINUX");
		#endif
	}

	public bool RT ()
	{
		#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
		return (Input.GetAxis ("RT_" + id) < triggerMagnitudeMin);
		#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
		return (Input.GetAxis ("MAC_RT_" + id) < triggerMagnitudeMin);
		#else
		Debug.Log("CONTROLS AREN'T DEFINED FOR LINUX");
		#endif
	}

	/*STICKS*/
	public float getLeftStickX ()
	{
		return Input.GetAxis ("LX_" + id);
	}

	public float getLeftStickY ()
	{
		return Input.GetAxis ("LY_" + id);
	}


	public float getRightStickX ()
	{
		#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
		return Input.GetAxis ("RX_" + id);
		#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
			return Input.GetAxis ("MAC_RX_" + id);
		#else
			Debug.Log("CONTROLS AREN'T DEFINED FOR LINUX");
		#endif
	}

	public float getRightStickY ()
	{
		#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
		return Input.GetAxis ("RY_" + id);
		#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
			return Input.GetAxis ("MAC_RY_" + id);
		#else
			Debug.Log("CONTROLS AREN'T DEFINED FOR LINUX");
		#endif
	}

	public float getDPadX ()
	{
		#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
		return Input.GetAxis ("DPADX_" + id);
		#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
		if( Input.GetKeyDown(Dpad_UP))
			return 1;
		else if( Input.GetKeyDown(Dpad_DOWN))
			return -1;
		else
			return 0;
		#else
			Debug.Log("CONTROLS AREN'T DEFINED FOR LINUX");
		#endif
	}

	public float getDPadY ()
	{
		#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
		return Input.GetAxis ("DPADY_" + id);
		#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
		if( Input.GetKeyDown(Dpad_RIGHT))
			return 1;
		else if( Input.GetKeyDown(Dpad_LEFT))
			return -1;
		else
			return 0;
		#else
			Debug.Log("CONTROLS AREN'T DEFINED FOR LINUX");
		#endif
	}

	public  Dictionary<string, KeyCode> getAllButtons ()
	{
		return allButtons;
	}

	public int getID ()
	{
		return id;
	}
}
