using UnityEngine;

public class RobotOverheatState : RobotFramedState {
    protected float InitialSpeed = 0;
    protected bool IsSpeedSet = false;
    protected int Duration = 100;
    protected GameObject Lightnings = null;

    protected override void Initialize() {
        this.IASA = this.MaxFrame;
        this.MinActiveState = 0;
        this.MaxActiveState = this.MaxFrame;
        this.HeatCost = 0;

        GameObject player = GameObject.FindGameObjectWithTag(
            PlayerController.Player);
        Transform playerTransform = player.GetComponent<Transform>();

        if (playerTransform == null) return;

        foreach (Transform child in playerTransform) {
            if (child.CompareTag("Lightnings")) {
                this.Lightnings = child.gameObject;
            }
        }
    }

    public RobotOverheatState() {
        this.MaxFrame = this.Duration;

        this.Initialize();
    }

    public override State HandleInput(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return null;

        RobotStateMachine robotStateMachine = (RobotStateMachine) stateMachine;

        if (!this.IsAnimationPlaying(
            robotStateMachine, "RobotOverheat")) {
            return null;
        }

        if (!this.IsSpeedSet) this.SetSpeed(robotStateMachine);

        if (!this.IsStateFinished()) return null;

        if (this.IsLastState(robotStateMachine, "RobotWalkState")) {
            return new RobotWalkState();
        }

        return new RobotIdleState();
    }

    public override void Update(StateMachine stateMachine) {
        this.CurrentFrame++;
    }

    public override void Enter(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return;

        RobotStateMachine robotStateMachine = (RobotStateMachine) stateMachine;

        this.InitialSpeed = robotStateMachine.Animator.speed;

        if (this.Lightnings != null) this.Lightnings.SetActive(true);
    }

    public override void Exit(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return;

        RobotStateMachine robotStateMachine = (RobotStateMachine) stateMachine;

        robotStateMachine.Animator.speed = this.InitialSpeed;

        if (this.Lightnings != null) this.Lightnings.SetActive(false);
    }

    protected virtual void SetSpeed(RobotStateMachine robotStateMachine) {
        robotStateMachine.Animator.speed = 2.5f;

        this.IsSpeedSet = true;
    }
}
