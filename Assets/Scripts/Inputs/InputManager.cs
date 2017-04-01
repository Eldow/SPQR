using UnityEngine;

public class InputManager : MonoBehaviour {
    private bool _dPadXInUse = false;
    private bool _dPadYInUse = false;
    private float _timer = 0f;
    private float _previousTime = 0f;

    void Update() {
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
        float r = 0f;
        r += Input.GetAxis("JoystickMove_Horizontal");
        r += Input.GetAxis("KeyboardMove_Horizontal");
        return Mathf.Clamp(r, -1.0f, 1.0f);
    }

    public float moveY() {
        float r = 0f;
        r += Input.GetAxis("JoystickMove_Vertical");
        r += Input.GetAxis("KeyboardMove_Vertical");
        return Mathf.Clamp(r, -1.0f, 1.0f);
    }

    public  float cameraX() {
        float r = 0f;
        r += Input.GetAxis("JoystickCamera_Horizontal");
        r += Input.GetAxis("MouseCamera_Horizontal");

        return Mathf.Clamp(r, -1.0f, 1.0f);
    }

    public  float cameraY() {
        float r = 0f;
        r += Input.GetAxis("JoystickCamera_Vertical");
        r += Input.GetAxis("MouseCamera_Vertical");

        return Mathf.Clamp(r, -1.0f, 1.0f);
    }

    public  bool switchCameraOffsetDown() {
        return Input.GetButtonDown("SwitchCameraOffset");
    }

    public  bool attackButton() {
        return Input.GetButtonDown("AttackButton");
    }

    public  bool powerAttackButtonDown() {
        return Input.GetButtonDown("PowerAttackButton");
    }

    public  bool powerAttackButton() {
        return Input.GetButton("PowerAttackButton");
    }

    public  bool dashButton()
    {
        return Input.GetButtonDown("DashButton");
    }

    public  bool runButton() {
        return Input.GetButton("RunButton") || 
            Mathf.Abs(Input.GetAxis("RunJoystick")) > 0.3f;
    }

    public  bool blockButton() {
        return Input.GetButton("BlockButton");
    }

    public  bool cameraButton() {
        return Input.GetButton("CameraButton");
    }

    public  bool cameraButtonDown() {
        return Input.GetButtonDown("CameraButton");
    }

    public  bool nextFrame() {
        return this.dPadRightDown() || 
            Input.GetButtonDown("NextFrame");
    }

    public  bool frameByFrame() {
        return this.dPadUpDown() || 
            Input.GetButtonDown("FrameByFrame");
    }

    public  bool dPadRightDown() {
        if (this._dPadXInUse) return false;

        this._dPadXInUse = Input.GetAxis("DPadX") > 0f;

        return this._dPadXInUse;
    }

    public  bool dPadLeftDown() {
        if (this._dPadXInUse) return false;

        this._dPadXInUse = Input.GetAxis("DPadX") < 0f;

        return this._dPadXInUse;
    }

    public  bool dPadUpDown() {
        if (this._dPadYInUse) return false;

        this._dPadYInUse = Input.GetAxis("DPadY") > 0f;

        return this._dPadYInUse;
    }

    public  bool dPadDownDown() {
        if (this._dPadYInUse) return false;

        this._dPadYInUse = Input.GetAxis("DPadY") < 0f;

        return this._dPadYInUse;
    }

    private  bool checkDPadAxis() {
        return _dPadXInUse || _dPadYInUse;
    }

    private  void resetDPadAxis() {
        this._dPadXInUse = Mathf.Abs(Input.GetAxis("DPadX")) > 0f;
        this._dPadYInUse = Mathf.Abs(Input.GetAxis("DPadY")) > 0f;
    }
}
