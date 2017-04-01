using UnityEngine;

public class FrameByFrame : MonoBehaviour {
	public Transform Target { get; protected set; }
    public PlayerController PlayerController { get; protected set; }
    public bool Enable = false;
    private bool _hasChanged = false;
	private InputManager inputManager;
	
	void Start() {
		FindTargetPlayer();
		inputManager = Target.gameObject.GetComponent<RobotStateMachine>().PlayerController.inputManager;
	}

    void Update() {
        this.CheckIfEnabled();

        if (this._hasChanged) {
            // enabling it the first frame the button has been pressed
            GameManager.Instance.Running.IsRunning = !this.Enable;

            this._hasChanged = false;
        } else {
            if (!this.Enable) return;

            GameManager.Instance.Running.IsRunning = false;

            if (!inputManager.nextFrame()) return;

            GameManager.Instance.Running.IsRunning = true;
        }
    }

    protected virtual void CheckIfEnabled() {
        if (!inputManager.frameByFrame()) {
            return;
        }

        this.Enable = !this.Enable;
        this._hasChanged = true;
    }
	
	protected void TryToGetPlayerController() {
        if (TargetManager.instance == null) return;

        GameObject player = TargetManager.instance.player;

        if (player == null) return;

        this.PlayerController =
            player.gameObject.GetComponent<PlayerController>();
    }
	
	public void FindTargetPlayer() {
        if (this.PlayerController == null) {
            this.TryToGetPlayerController();

            if (this.PlayerController == null) return;
        }

        this.Target = this.PlayerController.gameObject.transform;
		//inputManager inputManager = ((RobotStateMachine) stateMachine).PlayerController.inputManager;
        this.PlayerController.PlayerPhysics.IsLockedMovement = false;
    }
}
