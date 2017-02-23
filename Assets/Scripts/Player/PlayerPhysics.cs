using UnityEngine;

public class PlayerPhysics : Photon.MonoBehaviour {
    public Vector3 MoveDirection { get; protected set; }
    public Rigidbody RigidBody { get; protected set; }

    public float LockedForwardSpeed = 12f;
    public float LockedBackwardSpeed;
    public float UnlockedForwardSpeed = 12f;
    public float RunSpeed = 3f;
    public float TurnSpeed = 500f;
    public float DecelerationTweak = 2f;

    public bool IsLockedMovement = false;
    public float InputTolerance = 0.1f;

    private Transform _cameraHolder;
    private Transform _cameraTransform;
    private Quaternion _targetRotation;
    private Vector3 _targetDirection;
    private float _yAxisInput, _xAxisInput;
    private bool _isMoving = false;
    private bool _movingBack = false;

    void Start () {
        this.RigidBody = this.gameObject.GetComponent<Rigidbody>();

        this._cameraHolder = Camera.main.transform.parent.parent;
        this._cameraHolder.GetComponent<FreeCameraLook>().SetTarget(transform);

        this.RigidBody = this.gameObject.GetComponent<Rigidbody>();
        this._targetRotation = this.gameObject.transform.rotation;
        this._yAxisInput = this._xAxisInput = 0;
        this._cameraTransform = Camera.main.transform;
	}

    void Update() {
        this.GetInput();

        if (!this._isMoving) {
            this._isMoving = false;

            return;
        }

        this.StopRobot();
    }

    protected virtual void StopRobot() {
        Vector3 forward = _cameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;

        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        this._targetDirection =
            this._xAxisInput * right + this._yAxisInput * forward;
        this.MoveDirection = this._targetDirection.normalized;
        this.RigidBody.velocity = this.MoveDirection * this.DecelerationTweak;
        this._isMoving = false;
    }

    void GetInput() {
        this._yAxisInput = InputManager.moveY();
        this._xAxisInput = InputManager.moveX();
    }

    public void Movement(float multiplier = 1.0f) {
        this._isMoving = true;

        if (this.photonView.isMine) {
            if (!this.IsLockedMovement) {
                this.UnlockedMovement(multiplier);
            } else {
                this.LockedMovement(multiplier);
            }
        }

    }

    public void RunMovement() {
        this.Movement(this.RunSpeed);
    }

    public void LockedMovement(float multiplier = 1.0f) {
        if (Mathf.Abs(_yAxisInput) > InputTolerance) {
            this.RigidBody.velocity = 
                Camera.main.transform.forward * _yAxisInput * 
                LockedForwardSpeed * multiplier;
        } else {
            this.RigidBody.velocity = Vector3.zero;
        }
    }

    public void UnlockedMovement(float multiplier = 1) {

        this._movingBack = this._yAxisInput < -0.2;

        this.MoveDirection = Vector3.RotateTowards(
            this.MoveDirection, this._targetDirection, 
            this.TurnSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000
            );

        this.MoveDirection = this.MoveDirection.normalized;

        this.RigidBody.velocity = 
            this.MoveDirection * this.UnlockedForwardSpeed * multiplier;
      
        this.gameObject.transform.rotation = 
            Quaternion.RotateTowards(
                transform.rotation, 
                Quaternion.LookRotation(this.MoveDirection), 
                Time.deltaTime * 500
            );
    }
}
