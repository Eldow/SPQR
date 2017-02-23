using UnityEngine;

public class PlayerPhysics : Photon.MonoBehaviour {
    public Rigidbody RigidBody { get; protected set; }
    public Vector3 MoveDirection { get; protected set; }
    public Vector3 TargetDirection { get; protected set; }
    public Vector3 CameraForwardDirection { get; protected set; }
    public Vector3 CameraRightDirection { get; protected set; }
    public bool IsMoving { get; protected set; }
    public Transform CameraTransform { get; protected set; }

    public float LockedForwardSpeed = 12f;
    public float LockedBackwardSpeed;
    public float UnlockedForwardSpeed = 12f;
    public float RunSpeed = 3f;
    public float TurnSpeed = 500f;
    public float DecelerationTweak = 2f;
    public bool IsLockedMovement = false;
    public float InputTolerance = 0.1f;

    private float _yAxisInput = 0f;
    private float _xAxisInput = 0f;

    void Start () {
        this.RigidBody = this.gameObject.GetComponent<Rigidbody>();
        this.RigidBody = this.gameObject.GetComponent<Rigidbody>();
        this._yAxisInput = this._xAxisInput = 0;
        this.CameraTransform = Camera.main.transform;
        this.IsMoving = false;
    }

    void Update() {
        this.GetInput();

        if (!this.IsMoving) {
            this.IsMoving = false;

            this.StopRobot();

            return;
        }
    }

    protected virtual void StopRobot() {
        this.GetCameraVectors();
        this.GetTargetDirection();
        
        this.MoveDirection = this.TargetDirection.normalized;
        this.RigidBody.velocity = this.MoveDirection * this.DecelerationTweak;
        this.IsMoving = false;
    }

    protected virtual void GetInput() {
        this._yAxisInput = InputManager.moveY();
        this._xAxisInput = InputManager.moveX();
    }

    protected virtual void GetCameraVectors() {
        this.CameraForwardDirection = 
            this.CameraTransform.TransformDirection(Vector3.forward);

        this.CameraForwardDirection = new Vector3(
            this.CameraForwardDirection.x,
            0,
            this.CameraForwardDirection.z
        );

        this.CameraForwardDirection = this.CameraForwardDirection.normalized;

        this.CameraRightDirection = new Vector3(
            this.CameraForwardDirection.z, 
            0, 
            -this.CameraForwardDirection.x
        );
    }

    protected virtual void GetTargetDirection() {
        this.TargetDirection =
            this._xAxisInput * this.CameraRightDirection + this._yAxisInput * 
            this.CameraForwardDirection;
    }

    public void Move() {
        this.Movement();
    }

    public void Movement(float speedFactor = 1.0f) {
        this.IsMoving = true;

        if (!this.photonView.isMine) return;

        if (!this.IsLockedMovement) {
            this.UnlockedMovement(speedFactor);
        } else {
            this.LockedMovement(speedFactor);
        }
    }

    public void RunMovement() {
        this.Movement(this.RunSpeed);
    }

    public void LockedMovement(float speedFactor = 1.0f) {
        this.GetCameraVectors();
        this.GetTargetDirection();

        this.MoveDirection = this.TargetDirection.normalized;
        this.RigidBody.velocity = this.MoveDirection * this.LockedForwardSpeed;
        this.IsMoving = true;
    }

    public void UnlockedMovement(float speedFactor = 1) {
        this.GetCameraVectors();
        this.GetTargetDirection();

        this.MoveDirection = Vector3.RotateTowards(
            this.MoveDirection, this.TargetDirection, 
            this.TurnSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000
        );

        this.MoveDirection = this.MoveDirection.normalized;

        this.RigidBody.velocity = 
            this.MoveDirection * this.UnlockedForwardSpeed * speedFactor;
      
        this.gameObject.transform.rotation = 
            Quaternion.RotateTowards(
                transform.rotation, 
                Quaternion.LookRotation(this.MoveDirection), 
                Time.deltaTime * 500
            );
    }
}
