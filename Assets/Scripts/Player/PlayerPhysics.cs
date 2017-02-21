using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : Photon.MonoBehaviour {
    public GameObject CameraHolder;
    public float LockedForwardSpeed, LockedBackwardSpeed, LockedSidewaySpeed;
    public float UnlockedForwardSpeed, UnlockedBackwardSpeed, UnlockedRotationSpeed;
    public float BallRotationSpeed = 50f;
    public const float RunSpeed = 3f;  
    public bool IsLockedMovement = false;

    public float turnSpeed = 10f;

    private Vector3 _movement;
    private Rigidbody _rigidBody;

    /* Photon synchronization */
    private float _lastSynchronizationTime = 0f;
    private float _syncDelay = 0f;
    private float _syncTime = 0f;
    private Vector3 _syncStartPosition = Vector3.zero;
    private Vector3 _syncEndPosition = Vector3.zero;

    void Start () {
        this._rigidBody = GetComponent<Rigidbody>();
        CameraHolder.GetComponent<FreeCameraLook>().SetTarget(transform);
        _rigidBody = GetComponent<Rigidbody>();
	}

    public void Movement(float multiplier = 1) {
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
        float moveHorizontal = InputManager.moveX();
        float moveVertical = InputManager.moveY();

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        _rigidBody.velocity = movement * 5f;

        _rigidBody.position = new Vector3 (
            Mathf.Clamp(_rigidBody.position.x, -10f, 10f),
            0.0f,
            Mathf.Clamp(_rigidBody.position.z, -10f, 10f)
        );
    }

    /* Unlocked movement management : player translates with the left stick 
     * forward and backward and rotates around itself sideways
     */
    public void UnlockedMovement(float multiplier = 1) {
        float moveHorizontal = InputManager.moveX();
        float moveVertical = InputManager.moveY();

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        _rigidBody.velocity = movement * 5f;

        _rigidBody.position = new Vector3 (
            Mathf.Clamp(_rigidBody.position.x, -10f, 10f),
            0.0f,
            Mathf.Clamp(_rigidBody.position.z, -10f, 10f)
        );

        _rigidBody.rotation = Quaternion.Euler(0.0f, 0.0f, 
            _rigidBody.velocity.x * -0.5f);

        transform.rotation = Quaternion.Lerp(transform.rotation, 
            Quaternion.LookRotation(_rigidBody.velocity), 
            Time.fixedDeltaTime * turnSpeed);
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting) {
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
        }
    }
}
