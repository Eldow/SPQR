using UnityEngine;

public class PlayerPhysics : Photon.MonoBehaviour {
    public Rigidbody RigidBody { get; protected set; }
    public Vector3 MoveDirection { get; protected set; }
    public Vector3 TargetDirection { get; protected set; }
    public Vector3 CameraForwardDirection { get; protected set; }
    public Vector3 CameraRightDirection { get; protected set; }
    public bool IsMoving { get; protected set; }
    public Transform CameraTransform { get; protected set; }

	public bool freezeMovement = false;

    public float LockedForwardSpeed = 8f;
    public float LockedBackwardSpeed;
    public float UnlockedForwardSpeed = 8f;
    public float RunSpeed = 1.5f;
    public float DashSpeed = 15f;
    public float TurnSpeed = 500f;
    public float DecelerationTweak = 2f;
    public bool IsLockedMovement = false;
    public float InputTolerance = 0.1f;
    public float RunCap = 10f;
    public float WalkCap = 1f;
    public float MaximumSpeed = 300f;

    private float _yAxisInput = 0f;
    private float _xAxisInput = 0f;

    void Start () {
        this.Initialize();
    }

    void Update() {
		if (!freezeMovement) {
			this.UpdatePhysics ();
		}

      //  Debug.Log(RigidBody.velocity.sqrMagnitude);
    }

    protected virtual void Initialize() {
        this.RigidBody = this.gameObject.GetComponent<Rigidbody>();
        this.RigidBody = this.gameObject.GetComponent<Rigidbody>();
        this._yAxisInput = this._xAxisInput = 0;
        this.CameraTransform = Camera.main.transform;
        this.IsMoving = false;
    }

    protected virtual void UpdatePhysics() {
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

    public void Run() {
        this.Movement(this.RunSpeed);
    }

    public void Dash()
    {
        this.RigidBody.AddForce(this.MoveDirection * this.UnlockedForwardSpeed * DashSpeed,ForceMode.Acceleration);
    }

    public virtual void Movement(float speedFactor = 1.0f) {
        this.IsMoving = true;

        if (!photonView.isMine) return;

        if (!this.IsLockedMovement) {
            this.UnlockedMovement(speedFactor);
        } else {
            this.LockedMovement(speedFactor);
        }
    }

    public void LockedMovement(float speedFactor = 1.0f) {
        this.GetCameraVectors();
        this.GetTargetDirection();

        this.MoveDirection = this.TargetDirection.normalized;

        this.RigidBody.velocity =
            this.MoveDirection * this.UnlockedForwardSpeed * speedFactor;

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

        if (this.MoveDirection.sqrMagnitude <= 0.02f) return;
      
        this.gameObject.transform.rotation = 
            Quaternion.RotateTowards(
                transform.rotation, 
                Quaternion.LookRotation(this.MoveDirection), 
                Time.deltaTime * 500
            );
    }

    public virtual bool IsRunning()
    {
        return this.RigidBody.velocity.sqrMagnitude >= this.RunCap; 
    }

    public virtual bool IsWalking()
    {
        return this.RigidBody.velocity.sqrMagnitude >= this.WalkCap;
    }
}
