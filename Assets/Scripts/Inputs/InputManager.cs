using UnityEngine;

public class InputManager : MonoBehaviour {
    private static bool _dPadXInUse = false;
    private static bool _dPadYInUse = false;
    private static float _timer = 0f;
    private static float _previousTime = 0f;

    void Update() {
        InputManager._timer += 
            Time.realtimeSinceStartup - InputManager._previousTime;
        InputManager._previousTime = Time.realtimeSinceStartup;

        if (!(InputManager._timer - Time.fixedDeltaTime > 0f) ||
            !InputManager.checkDPadAxis()) {
            return;
        }

        InputManager.resetDPadAxis();
        InputManager._timer = 0f;
    }

    public static float moveX() {
        float r = 0f;
        r += Input.GetAxis("JoystickMove_Horizontal");
        r += Input.GetAxis("KeyboardMove_Horizontal");
        return Mathf.Clamp(r, -1.0f, 1.0f);
    }

    public static float moveY() {
        float r = 0f;
        r += Input.GetAxis("JoystickMove_Vertical");
        r += Input.GetAxis("KeyboardMove_Vertical");
        return Mathf.Clamp(r, -1.0f, 1.0f);
    }

    public static float cameraX() {
        float r = 0f;
        r += Input.GetAxis("JoystickCamera_Horizontal");
        r += Input.GetAxis("MouseCamera_Horizontal");

        return Mathf.Clamp(r, -1.0f, 1.0f);
    }

    public static float cameraY() {
        float r = 0f;
        r += Input.GetAxis("JoystickCamera_Vertical");
        r += Input.GetAxis("MouseCamera_Vertical");

        return Mathf.Clamp(r, -1.0f, 1.0f);
    }

    public static bool switchCameraOffsetDown() {
        return Input.GetButtonDown("SwitchCameraOffset");
    }

    public static bool attackButton() {
        return Input.GetButtonDown("AttackButton");
    }

    public static bool dashButton()
    {
        return Input.GetButtonDown("DashButton");
    }

    public static bool runButton() {
        return Input.GetButton("RunButton") || 
            Mathf.Abs(Input.GetAxis("RunJoystick")) > 0.3f;
    }

    public static bool blockButton() {
        return Input.GetButton("BlockButton");
    }

    public static bool cameraButton() {
        return Input.GetButton("CameraButton");
    }

    public static bool cameraButtonDown() {
        return Input.GetButtonDown("CameraButton");
    }

    public static bool nextFrame() {
        return InputManager.dPadRightDown() || 
            Input.GetButtonDown("NextFrame");
    }

    public static bool frameByFrame() {
        return InputManager.dPadUpDown() || 
            Input.GetButtonDown("FrameByFrame");
    }

    public static bool dPadRightDown() {
        if (InputManager._dPadXInUse) return false;

        InputManager._dPadXInUse = Input.GetAxis("DPadX") > 0f;

        return InputManager._dPadXInUse;
    }

    public static bool dPadLeftDown() {
        if (InputManager._dPadXInUse) return false;

        InputManager._dPadXInUse = Input.GetAxis("DPadX") < 0f;

        return InputManager._dPadXInUse;
    }

    public static bool dPadUpDown() {
        if (InputManager._dPadYInUse) return false;

        InputManager._dPadYInUse = Input.GetAxis("DPadY") > 0f;

        return InputManager._dPadYInUse;
    }

    public static bool dPadDownDown() {
        if (InputManager._dPadYInUse) return false;

        InputManager._dPadYInUse = Input.GetAxis("DPadY") < 0f;

        return InputManager._dPadYInUse;
    }

    private static bool checkDPadAxis() {
        return _dPadXInUse || _dPadYInUse;
    }

    private static void resetDPadAxis() {
        InputManager._dPadXInUse = Mathf.Abs(Input.GetAxis("DPadX")) > 0f;
        InputManager._dPadYInUse = Mathf.Abs(Input.GetAxis("DPadY")) > 0f;
    }
}
