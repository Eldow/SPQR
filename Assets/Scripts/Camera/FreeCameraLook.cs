using UnityEngine;

public class FreeCameraLook : Pivot {
	
    public float MoveSpeed = 5f;
    public float TurnSpeed = 1.5f;
    public float TurnSmoothing = .1f;
    public float TiltMax = 75f;
    public float TiltMin = 45f;
    public bool LockCursor = false;
    public float TiltAngle;
    public float SmoothX = 0;
    public float SmoothY = 0;
    public float SmoothXvelocity = 0;
    public float SmoothYvelocity = 0;
    public Vector3 PivotOffset;

    public Vector3 PivotLockLeftAngles;
    protected Vector3 PivotLockRightAngles;
    protected Vector3 PivotLockAngles;


    protected override void Initialize() {
		
        this.PivotLockLeftAngles = new Vector3(
            12, 20, 0);
        this.PivotLockAngles = this.PivotLockLeftAngles;
        this.PivotLockRightAngles = new Vector3(
            this.PivotLockLeftAngles.x,
            -this.PivotLockLeftAngles.y,
            -this.PivotLockLeftAngles.z
        );

        base.Initialize();

        Cursor.lockState = CursorLockMode.Confined;
    }

    void Start() {
        this.Initialize();
    }

    void Update() {
		if (inputManager == null) {
			this.FindTargetPlayer ();
			if (Target != null) {
				inputManager = Target.gameObject.GetComponent<RobotStateMachine> ().PlayerController.inputManager;
				if (inputManager == null) {
					return;
				} 
			} else {
				return;
			}
		} 

        this.UpdateTarget();

        if (GameManager.Instance == null) return;

        if (!GameManager.Instance.Running.IsRunning) return;

        if (this.LockCamera) {
            if (inputManager.switchCameraOffsetDown()) {
				ApplyOffset ();
            }

            this.RotateLockCamera();
        } else {
            this.HandleRotationMovement();
        }

        if (this.LockCursor && Input.GetMouseButtonUp(0)) {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    void OnDisable() {
        Cursor.lockState = CursorLockMode.None;
    }

    protected virtual void SwitchPivotSide() {
        this.UndoOffset();

		this.PivotObject.localRotation = Quaternion.Euler(this.MainCameraDefaultLockRotation);

        this.PivotObject.transform.localPosition = new Vector3(
            -this.PivotObject.transform.localPosition.x,
            this.PivotObject.transform.localPosition.y,
            this.PivotObject.transform.localPosition.z
        );

        if (this.IsLeftPivot) {
            this.PivotLockAngles = this.PivotLockRightAngles;
            this.MainCameraOffset = this.MainCameraRightOffset;
        } else {
            this.PivotLockAngles = this.PivotLockLeftAngles;
            this.MainCameraOffset = this.MainCameraLeftOffset;
        }

   

        this.IsLeftPivot = !this.IsLeftPivot;
    }

    protected virtual void RotateLockCamera() {
        if (this.OpponentController == null) {
            //this.SwitchCameraMode(); useless ?
            return;
        }

        this.transform.LookAt(this.OpponentController.transform);
    }

    protected override void UndoOffset() {

		this.TiltAngle = 0;

        this.PivotObject.transform.localPosition = this.PivotDefaultPosition;
        this.PivotObject.transform.localRotation =
            Quaternion.Euler(this.PivotDefaultRotation);

        base.UndoOffset();

		Quaternion temp = this.transform.rotation;
		temp.x = 0;
		temp.z = 0;
			this.transform.rotation = temp;
    }

    protected override void ApplyOffset() {
		SwitchPivotSide ();
        this.PivotObject.transform.localPosition = this.PivotDefaultPosition;
        this.PivotObject.transform.localRotation = 
            Quaternion.Euler(this.PivotDefaultRotation);

        base.ApplyOffset();
    }

    protected override void Follow(float deltaTime, bool lockCam) {
        this.LockCamera = lockCam;
        this.transform.position =
            Vector3.Lerp(
                this.transform.position,
                this.Target.position,
                deltaTime * this.MoveSpeed
            );
    }

    void HandleRotationMovement() {
        float cameraX = inputManager.cameraX();
        float cameraY = inputManager.cameraY();

        // prevent the camera from turning back when quitting the lock mode
        if (Mathf.Abs(cameraX) <= 0.02f && Mathf.Abs(cameraY) <= 0.02f) {
            return;
        }

        if (TurnSmoothing > 0) {
            this.SmoothX = Mathf.SmoothDamp(
                this.SmoothX, cameraX, ref this.SmoothXvelocity, 
                this.TurnSmoothing
            );

            this.SmoothY = Mathf.SmoothDamp(
                this.SmoothY, cameraY, ref this.SmoothYvelocity, 
                this.TurnSmoothing
            );
        } else {
            this.SmoothX = cameraX;
            this.SmoothY = cameraY;
        }
			
		this.transform.Rotate (Vector3.up * this.SmoothX * this.TurnSpeed, Space.World);

        this.TiltAngle -= this.SmoothY * this.TurnSpeed;
        this.TiltAngle = Mathf.Clamp(
            this.TiltAngle, -this.TiltMin, this.TiltMax);

        this.PivotObject.localRotation = Quaternion.Euler(
            this.TiltAngle, 0, 0);
    }
}
