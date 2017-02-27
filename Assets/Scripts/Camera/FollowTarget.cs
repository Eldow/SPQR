using UnityEngine;

public abstract class FollowTarget : MonoBehaviour {    
    public bool AutoTarget = true;
    public bool LockCamera = false;

    public Vector3 MainCameraLeftOffset;
    public Vector3 MainCameraRightOffset;
    public Vector3 MainCameraOffset;
    public Vector3 MainCameraDefaultUnlockPosition;
    public Vector3 MainCameraDefaultUnlockRotation;
    public Vector3 MainCameraDefaultLockPosition;
    public Vector3 MainCameraDefaultLockRotation;
    protected bool IsLeftPivot = true;
    protected bool OffsetApplied = false;

    public Transform Target { get; protected set; }
    public PlayerController PlayerController { get; protected set; }
    public NetPlayerController OpponentController { get; protected set; }
    public GameObject Opponent { get; protected set; }
    public GameObject HealthBar { get; protected set; }

    protected virtual void Initialize() {
        this.MainCameraLeftOffset = new Vector3(-1, -3, 3);
        this.MainCameraOffset = this.MainCameraLeftOffset;
        this.MainCameraRightOffset = new Vector3(
            -this.MainCameraLeftOffset.x,
            this.MainCameraLeftOffset.y,
            this.MainCameraLeftOffset.z
        );

        this.MainCameraDefaultLockPosition = new Vector3(0, 2, -8);
        this.MainCameraDefaultLockRotation = new Vector3(8, 0, 0);

        this.MainCameraDefaultUnlockPosition = new Vector3(0, 0, -8.75f);
        this.MainCameraDefaultUnlockRotation = new Vector3(0, 0, 0);


        this.TryToGetPlayerController();

        if (this.AutoTarget) {
            this.FindTargetPlayer();
        }
    }

    void Start() {
        this.Initialize();
    }

    void Update() {
        this.UpdateTarget();
    }

    protected virtual void ApplyOffset() {
        Camera.main.transform.localPosition = this.MainCameraDefaultLockPosition;
        Camera.main.transform.localRotation = 
            Quaternion.Euler(this.MainCameraDefaultLockRotation);

        Camera.main.transform.localPosition = new Vector3(
            Camera.main.transform.localPosition.x + this.MainCameraOffset.x,
            Camera.main.transform.localPosition.y + this.MainCameraOffset.y,
            Camera.main.transform.localPosition.z + this.MainCameraOffset.z
        );

        this.OffsetApplied = true;
    }

    protected virtual void UndoOffset() {
        Camera.main.transform.localPosition = this.MainCameraDefaultLockPosition;
        Camera.main.transform.localRotation =
            Quaternion.Euler(this.MainCameraDefaultLockRotation);
    }

    protected virtual void UpdateTarget() {
        if (this.CheckIfPlayerToFind()) {
            this.FindTargetPlayer();
        }

        if (this.PlayerController == null) return;

        this.UpdateOpponent();

        if (InputManager.cameraButtonDown() && 
            this.OpponentController != null) {
            this.SwitchCameraMode();
        }

        if (this.LockCamera) {
            this.LookAtOpponent();
        } else {
            this.FindTargetPlayer();
        }

        this.Follow(Time.deltaTime, LockCamera);
    }

    protected void TryToGetPlayerController() {
        if (TargetManager.instance == null) return;

        GameObject player = TargetManager.instance.player;

        if (player == null) return;

        this.PlayerController =
            player.gameObject.GetComponent<PlayerController>();
    }

    protected bool CheckIfPlayerToFind() {
        return this.AutoTarget &&
               (this.Target == null || !this.Target.gameObject.activeSelf);
    }

    protected abstract void Follow(float deltaTime, bool lockCamera);

    public void FindTargetPlayer() {
        if (this.PlayerController == null) {
            this.TryToGetPlayerController();

            if (this.PlayerController == null) return;
        }

        this.Target = this.PlayerController.gameObject.transform;
        this.PlayerController.PlayerPhysics.IsLockedMovement = false;
    }

    public void LookAtOpponent() {
        if (this.PlayerController == null) {
            this.TryToGetPlayerController();

            if (this.PlayerController == null) return;
        }

        this.UpdateOpponent();

        Quaternion neededRotation;

        if (this.OpponentController != null) {
            neededRotation = Quaternion.LookRotation(
                this.OpponentController.transform.position - 
                this.PlayerController.gameObject.transform.position
            );
        } else {
            neededRotation = Quaternion.LookRotation(
                this.PlayerController.gameObject.transform.forward,
                this.PlayerController.gameObject.transform.up
            );
        }

        this.PlayerController.gameObject.transform.rotation = Quaternion.Slerp(
            this.PlayerController.gameObject.transform.rotation, 
            neededRotation, 
            Time.deltaTime * 5f
        );

        this.PlayerController.PlayerPhysics.IsLockedMovement = true;
    }

    protected void UpdateOpponent() {
        GameObject opponent = TargetManager.instance.GetNearestOpponent();

        if (opponent == null) return;

        this.OpponentController = opponent.GetComponent<NetPlayerController>();
    }

    protected virtual void ResetCameraUnlockPosition() {
        Camera.main.transform.localPosition = this.MainCameraDefaultUnlockPosition;
        Camera.main.transform.localRotation =
            Quaternion.Euler(this.MainCameraDefaultUnlockRotation);
    }

    public virtual void SwitchCameraMode() {
        this.LockCamera = !this.LockCamera;

        if (!this.LockCamera) {
            this.UndoOffset();
            this.ResetCameraUnlockPosition();

            return;
        }

        this.ApplyOffset();

        if (this.OpponentController != null) {
            HealthBar = this.OpponentController.OpponentInfo;
            HealthBar.SetActive(LockCamera);
        }
    }
}
