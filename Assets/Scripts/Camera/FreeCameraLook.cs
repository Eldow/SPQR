using UnityEngine;

public class FreeCameraLook : Pivot {
    public float MoveSpeed = 5f;
    public float TurnSpeed = 1.5f;
    public float TurnSmoothing = .1f;
    public float TiltMax = 75f;
    public float TiltMin = 45f;
    public bool LockCursor = false;
    public float LookAngle;
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
        this.UpdateTarget();

        if (GameManager.Instance == null) return;

        if (!GameManager.Instance.Running.IsRunning) return;

        if (this.LockCamera) {
            if (InputManager.switchCameraOffsetDown()) {
                this.SwitchPivotSide();
            }

            this.RotateLockCamera();
        } else {
            if (InputManager.switchCameraOffsetDown()) {
                this.ApplyOffset();
            }

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

        this.PivotObject.localRotation = Quaternion.Euler(
            this.PivotLockAngles.x, 
            -this.PivotLockAngles.y, 
            this.PivotLockAngles.z
        );

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

        this.ApplyOffset();

        this.IsLeftPivot = !this.IsLeftPivot;
    }

    protected virtual void RotateLockCamera() {
        this.transform.LookAt(this.OpponentController.transform);
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

    protected override void UndoOffset() {
        base.ApplyOffset();
    }

    void HandleRotationMovement() {
        float cameraX = InputManager.cameraX();
        float cameraY = InputManager.cameraY();

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

        this.LookAngle += this.SmoothX * this.TurnSpeed;
        this.transform.rotation = Quaternion.RotateTowards(
            this.transform.rotation, 
            Quaternion.Euler(0f, this.LookAngle, 0), 
            this.TurnSpeed
        );

        this.TiltAngle -= this.SmoothY * this.TurnSpeed;
        this.TiltAngle = Mathf.Clamp(
            this.TiltAngle, -this.TiltMin, this.TiltMax);

        this.PivotObject.localRotation = Quaternion.Euler(
            this.TiltAngle, 0, 0);
    }
}
