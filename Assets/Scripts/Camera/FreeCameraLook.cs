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

    protected override void Initialize() {
        base.Initialize();

        Cursor.lockState = CursorLockMode.Confined;
    }

    void Start() {
        this.Initialize();
    }

    void Update() {
        if (GameManager.Instance == null) return;

        if (!GameManager.Instance.Running.IsRunning) return;

        if (this.LockCamera) {
            this.transform.rotation =
                Quaternion.Lerp(
                    this.transform.rotation,
                    Quaternion.LookRotation(
                        TargetManager.instance.player.transform.forward,
                        Vector3.zero
                    ),
                    Time.deltaTime * 20
                );
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
