using UnityEngine;

public class InputManager : MonoBehaviour {
    private bool _dPadXInUse = false;
    private bool _dPadYInUse = false;
    private float _timer = 0f;
    private float _previousTime = 0f;
	private bool isAI = false;
	public bool blockInputs = false;
	public RoundTimer roundTimer=null;
	
	//input simulator
	[HideInInspector] public bool attackButtonAI = false;
	[HideInInspector] public bool blockButtonAI = false;
	[HideInInspector] public float moveForwardSpeedAI = 0f;
	[HideInInspector] public float moveSideSpeedAI = 0f;

	void Start() {
		if(gameObject.GetComponent<AI>()){isAI = true;}
		GameObject temp = GameObject.Find ("RoundTimer");
		if(temp!=null)
			roundTimer = temp.GetComponent<RoundTimer> ();
	}
	
    void Update() {
		if (roundTimer != null) {
			blockInputs = !roundTimer.hasTimerStarted;
		}
        this._timer += 
            Time.realtimeSinceStartup - this._previousTime;
        this._previousTime = Time.realtimeSinceStartup;

        if (!(this._timer - Time.fixedDeltaTime > 0f) ||
            !this.checkDPadAxis()) {
            return;
        }

        this.resetDPadAxis();
        this._timer = 0f;
    }

    public float moveX() {
		if (blockInputs)
			return 0f;
        float r = 0f;
		if(!isAI){
			r += Input.GetAxis("JoystickMove_Horizontal");
			r += Input.GetAxis("KeyboardMove_Horizontal");
			return Mathf.Clamp(r, -1.0f, 1.0f);
		}
		return moveSideSpeedAI;
    }

    public float moveY() {
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

    public  float cameraX() {
		if (blockInputs)
			return 0f;
        float r = 0f;
        r += Input.GetAxis("JoystickCamera_Horizontal");
        r += Input.GetAxis("MouseCamera_Horizontal");

        return Mathf.Clamp(r, -1.0f, 1.0f);
    }

    public  float cameraY() {
		if (blockInputs)
			return 0f;
        float r = 0f;
        r += Input.GetAxis("JoystickCamera_Vertical");
        r += Input.GetAxis("MouseCamera_Vertical");

        return Mathf.Clamp(r, -1.0f, 1.0f);
    }

    public  bool switchCameraOffsetDown() {
		if (blockInputs)
			return false;
        return Input.GetButtonDown("SwitchCameraOffset");
    }

    public  bool attackButton() {
		if (blockInputs)
			return false;
		if(!isAI)
			return Input.GetButtonDown("AttackButton");
		return attackButtonAI;
    }

    public  bool powerAttackButtonDown() {
		if (blockInputs)
			return false;
        return Input.GetButtonDown("PowerAttackButton");
    }

    public  bool powerAttackButton() {
		if (blockInputs)
			return false;
        return Input.GetButton("PowerAttackButton");
    }
		
    public bool dischargeButton() {
		if (blockInputs)
			return false;
        return Input.GetButtonDown("DischargeButton");
    }


    public  bool dashButton()
    {
		if (blockInputs)
			return false;
        return Input.GetButtonDown("DashButton");
    }

    public  bool runButton() {
		if (blockInputs)
			return false;
        return Input.GetButton("RunButton") || 
            Mathf.Abs(Input.GetAxis("RunJoystick")) > 0.3f;
    }

    public  bool blockButton() {
		if (blockInputs)
			return false;
		if(!isAI)
			return Input.GetButton("BlockButton");
		return blockButtonAI;
    }

    public  bool cameraButton() {
		if (blockInputs)
			return false;
        return Input.GetButton("CameraButton");
    }

    public  bool cameraButtonDown() {
		if (blockInputs)
			return false;
        return Input.GetButtonDown("CameraButton");
    }

    public  bool nextFrame() {
		if (blockInputs)
			return false;
        return this.dPadRightDown() || 
            Input.GetButtonDown("NextFrame");
    }

    public  bool frameByFrame() {
		if (blockInputs)
			return false;
        return this.dPadUpDown() || 
            Input.GetButtonDown("FrameByFrame");
    }

    public  bool dPadRightDown() {
		if (blockInputs)
			return false;
        if (this._dPadXInUse) return false;

        this._dPadXInUse = Input.GetAxis("DPadX") > 0f;

        return this._dPadXInUse;
    }

    public  bool dPadLeftDown() {
		if (blockInputs)
			return false;
		
        if (this._dPadXInUse) return false;

        this._dPadXInUse = Input.GetAxis("DPadX") < 0f;

        return this._dPadXInUse;
    }

    public  bool dPadUpDown() {
		if (blockInputs)
			return false;
		
        if (this._dPadYInUse) return false;

        this._dPadYInUse = Input.GetAxis("DPadY") > 0f;

        return this._dPadYInUse;
    }

    public  bool dPadDownDown() {
		if (blockInputs)
			return false;
		
        if (this._dPadYInUse) return false;

        this._dPadYInUse = Input.GetAxis("DPadY") < 0f;

        return this._dPadYInUse;
    }

    private  bool checkDPadAxis() {
		if (blockInputs)
			return false;
		
        return _dPadXInUse || _dPadYInUse;
    }

    private  void resetDPadAxis() {
        this._dPadXInUse = Mathf.Abs(Input.GetAxis("DPadX")) > 0f;
        this._dPadYInUse = Mathf.Abs(Input.GetAxis("DPadY")) > 0f;
    }
}
