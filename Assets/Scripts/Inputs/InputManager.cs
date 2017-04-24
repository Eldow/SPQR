using UnityEngine;

public class InputManager : MonoBehaviour
{
	private bool _dPadXInUse = false;
	private bool _dPadYInUse = false;
	private float _timer = 0f;
	private float _previousTime = 0f;
	public bool isAI = false;
	public bool blockInputs = false;
	public RoundTimer roundTimer = null;

	//input simulator
	[HideInInspector] public bool attackButtonAI = false;
	[HideInInspector] public bool blockButtonAI = false;
	[HideInInspector] public float moveForwardSpeedAI = 0f;
	[HideInInspector] public float moveSideSpeedAI = 0f;

	void Start ()
	{

		PlayerController pc = gameObject.GetComponent<PlayerController> ();
		if (pc != null && pc.isAI) {
			isAI = true;
		}
		GameObject temp = GameObject.Find ("RoundTimer");
		if (temp != null)
			roundTimer = temp.GetComponent<RoundTimer> ();
	}

	void Update ()
	{
		if (roundTimer != null) {
			blockInputs = !roundTimer.hasTimerStarted;
		}
		this._timer += 
            Time.realtimeSinceStartup - this._previousTime;
		this._previousTime = Time.realtimeSinceStartup;

		if (!(this._timer - Time.fixedDeltaTime > 0f) ||
		    !this.checkDPadAxis ()) {
			return;
		}

		this.resetDPadAxis ();
		this._timer = 0f;
	}

	public float moveX ()
	{
		if (blockInputs)
			return 0f;
		float r = 0f;
		if (!isAI) {
			r += Input.GetAxis ("JoystickMove_Horizontal");
			r += Input.GetAxis ("KeyboardMove_Horizontal");
			return Mathf.Clamp (r, -1.0f, 1.0f);
		}
		return moveSideSpeedAI;
	}

	public float moveY ()
	{
		if (blockInputs)
			return 0f;
		
		float r = 0f;
		if (!isAI) {
			r += Input.GetAxis ("JoystickMove_Vertical");
			r += Input.GetAxis ("KeyboardMove_Vertical");
			return Mathf.Clamp (r, -1.0f, 1.0f);
		}
		
		return moveForwardSpeedAI;
	}

	public  float cameraX ()
	{
		if (blockInputs)
			return 0f;

		if (!isAI) {
			float r = 0f;
			r += Input.GetAxis ("JoystickCamera_Horizontal");
			r += Input.GetAxis ("MouseCamera_Horizontal");

			return Mathf.Clamp (r, -1.0f, 1.0f);
		}
		return 0f;
	}

	public  float cameraY ()
	{
		if (blockInputs)
			return 0f;
		if (!isAI) {
			float r = 0f;
			r += Input.GetAxis ("JoystickCamera_Vertical");
			r += Input.GetAxis ("MouseCamera_Vertical");

			return Mathf.Clamp (r, -1.0f, 1.0f);
		}
		return 0f;
	}

	public  bool switchCameraOffsetDown ()
	{
		if (blockInputs)
			return false;
		if (!isAI)
			return Input.GetButtonDown ("SwitchCameraOffset");

		return false;
	}

	public  bool attackButton ()
	{
		if (blockInputs)
			return false;
		if (!isAI)
			return Input.GetButtonDown ("AttackButton");
		return attackButtonAI;
	}

	public  bool powerAttackButtonDown ()
	{
		if (blockInputs)
			return false;

		if (!isAI)
			return Input.GetButtonDown ("PowerAttackButton");
		
		return false;
	}

	public  bool powerAttackButton ()
	{
		if (blockInputs)
			return false;
		if (!isAI)
			return Input.GetButton ("PowerAttackButton");
		return false;
	}

	public bool dischargeButton ()
	{
		if (blockInputs)
			return false;
		if (!isAI)
			return Input.GetButtonDown ("DischargeButton");
		return false;
	}


	public  bool dashButton ()
	{
		if (blockInputs)
			return false;
		if (!isAI)
			return Input.GetButtonDown ("DashButton");
		return false;
	}

	public  bool runButton ()
	{
		if (blockInputs)
			return false;
		if (!isAI)
			return Input.GetButton ("RunButton") ||
			Mathf.Abs (Input.GetAxis ("RunJoystick")) > 0.3f;

		return false;
	}

	public  bool blockButton ()
	{
		if (blockInputs)
			return false;
		if (!isAI)
			return Input.GetButton ("BlockButton");
		return blockButtonAI;
	}

	public  bool cameraButton ()
	{
		if (blockInputs)
			return false;

		if (!isAI)
		return Input.GetButton ("CameraButton");

		return false;
	}

	public  bool cameraButtonDown ()
	{
		if (blockInputs)
			return false;

		if (!isAI)
		return Input.GetButtonDown ("CameraButton");

		return false;
	}

    public bool menuButton()
    {
        if (!isAI)
            return Input.GetButtonDown("MenuButton");
        return false;
    }

    public bool infoButton()
    {
        if (!isAI)
            return Input.GetButtonDown("InfoButton");
        return false;
    }

	public  bool nextFrame ()
	{
		if (blockInputs)
			return false;
		return this.dPadRightDown () ||
		Input.GetButtonDown ("NextFrame");
	}

	public  bool frameByFrame ()
	{
		if (blockInputs)
			return false;
		return this.dPadUpDown () ||
		Input.GetButtonDown ("FrameByFrame");
	}

	public  bool dPadRightDown ()
	{
		if (blockInputs)
			return false;
		if (this._dPadXInUse)
			return false;

		this._dPadXInUse = Input.GetAxis ("DPadX") > 0f;
		if (!isAI)
			return this._dPadXInUse;

		return false;
	}

	public  bool dPadLeftDown ()
	{
		if (blockInputs)
			return false;
		
		if (this._dPadXInUse)
			return false;

		this._dPadXInUse = Input.GetAxis ("DPadX") < 0f;
		if (!isAI)
			return this._dPadXInUse;

		return false;
	}

	public  bool dPadUpDown ()
	{
		if (blockInputs)
			return false;
		
		if (this._dPadYInUse)
			return false;

		this._dPadYInUse = Input.GetAxis ("DPadY") > 0f;

		if (!isAI)
			return this._dPadYInUse;

		return false;
	}

	public  bool dPadDownDown ()
	{
		if (blockInputs)
			return false;
		
		if (this._dPadYInUse)
			return false;

		this._dPadYInUse = Input.GetAxis ("DPadY") < 0f;

		if (!isAI)
			return this._dPadYInUse;

		return false;
	}

	private  bool checkDPadAxis ()
	{
		if (blockInputs)
			return false;

		if (!isAI)
			return _dPadXInUse || _dPadYInUse;
		return false;
	}

	private  void resetDPadAxis ()
	{
		this._dPadXInUse = Mathf.Abs (Input.GetAxis ("DPadX")) > 0f;
		this._dPadYInUse = Mathf.Abs (Input.GetAxis ("DPadY")) > 0f;
	}
}
