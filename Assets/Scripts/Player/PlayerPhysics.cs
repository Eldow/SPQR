using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : Photon.MonoBehaviour {
    public float LockedForwardSpeed = 12f;
    public float LockedBackwardSpeed;
    public float UnlockedForwardSpeed = 12f;
    public float UnlockedBackwardSpeed;
    public float BallRotationSpeed = 50f;
    public const float RunSpeed = 3f;  
    public bool IsLockedMovement = false;
    public float InputDelay = 0.1f;
    public float TurnSpeed = 500f;

    private Transform _cameraHolder;
    private Transform _cameraTransform;
    private Vector3 _moveDirection;
    private Rigidbody _rigidBody;
    private Quaternion _targetRotation;
    private Vector3 _targetDirection;
    private float _forwardInput, _turnInput;
    private bool _movingBack;
    private bool _isMoving = false;
    /* Photon synchronization */
    private float _lastSynchronizationTime = 0f;
    private float _syncDelay = 0f;
    private float _syncTime = 0f;
    private Vector3 _syncStartPosition = Vector3.zero;
    private Vector3 _syncEndPosition = Vector3.zero;

    void Start () {
        this._rigidBody = GetComponent<Rigidbody>();
        _cameraHolder = Camera.main.transform.parent.parent;
        _cameraHolder.GetComponent<FreeCameraLook>().SetTarget(transform);
        _rigidBody = GetComponent<Rigidbody>();
        _targetRotation = transform.rotation;
        _forwardInput = _turnInput = 0;
        _cameraTransform = Camera.main.transform;
	}

    void Update()
    {
        GetInput();
        //Turn();
        if (!_isMoving)
        {
            _isMoving = false;
            return;
        }
        Vector3 forward = _cameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;
        Vector3 right = new Vector3(forward.z, 0, -forward.x);
        _targetDirection = _turnInput * right + _forwardInput * forward;
        _moveDirection = _targetDirection.normalized;
        _isMoving = false;
    }

    void GetInput()
    {
        _forwardInput = InputManager.moveY();
        _turnInput = InputManager.moveX();
    }

    void Turn()
    {
        if(Mathf.Abs(_turnInput) > InputDelay)
        {
            _targetRotation *= Quaternion.AngleAxis(TurnSpeed * _turnInput * Time.deltaTime, Vector3.up);
        }
        transform.rotation = _targetRotation;
    }

    public void Movement(float multiplier = 1) {
        _isMoving = true;
        if (photonView.isMine) {
            // Player's movement
            if (!IsLockedMovement) {
                UnlockedMovement(multiplier);
            } else {
                LockedMovement(multiplier);
            }
        }

    }

    public void RunMovement() {
        Movement(RunSpeed);
    }

    /* Locked movement management : player translates with the left stick 
     * forward, backward and sideways
     */
    public void LockedMovement(float multiplier = 1) {
        if (Mathf.Abs(_forwardInput) > InputDelay)
        {
            _rigidBody.velocity = Camera.main.transform.forward * _forwardInput * LockedForwardSpeed * multiplier;
        }
        else
        {
            _rigidBody.velocity = Vector3.zero;
        }
    }

    /* Unlocked movement management : player translates with the left stick 
     * forward and backward and rotates around itself sideways
     */
    public void UnlockedMovement(float multiplier = 1) {
   
        if (_forwardInput < -0.2)
            _movingBack = true;
        else
            _movingBack = false;


        // Target direction relative to the camera

        Debug.Log(_forwardInput + " forwardINPUT" + _turnInput + " turnINPUT" + InputDelay);
        if(Mathf.Abs(_forwardInput) < InputDelay && Mathf.Abs(_turnInput) < InputDelay)
        {
            
            Debug.Log("onche");
        } else
        {
            //_moveDirection = Vector3.RotateTowards(_moveDirection, _targetDirection, TurnSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);
            _moveDirection = new Vector3(_turnInput, 0, _forwardInput);
            Debug.Log("onche2");

        }

        _moveDirection = _moveDirection.normalized;

        _rigidBody.velocity = _moveDirection * UnlockedForwardSpeed * multiplier;
        Debug.Log(_rigidBody.velocity);


        if(Mathf.Abs(_turnInput) > InputDelay)
        {
            //_rigidBody.velocity = _
        }

        Vector3 characterDirection = new Vector3(_turnInput, 0, _forwardInput).normalized;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(characterDirection), Time.deltaTime * 500);
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        /*if (stream.isWriting) {
            stream.SendNext(_rigidBody.position);
            stream.SendNext(_rigidBody.velocity);
        } else {
            Vector3 syncPosition = (Vector3)stream.ReceiveNext();
            Vector3 syncVelocity = (Vector3)stream.ReceiveNext();

            _syncTime = 0f;
            _syncDelay = Time.time - _lastSynchronizationTime;
            _lastSynchronizationTime = Time.time;

            _syncEndPosition = syncPosition + syncVelocity * _syncDelay;
            _syncStartPosition = _rigidBody.position;
        }*/
    }
}
