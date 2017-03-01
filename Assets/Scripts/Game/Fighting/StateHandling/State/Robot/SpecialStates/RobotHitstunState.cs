using UnityEngine;

public class RobotHitstunState : RobotFramedState {
    protected float InitialSpeed = 0;
    protected bool IsSpeedSet = false;

    protected override void Initialize() {
        this.IASA = this.MaxFrame;
        this.MinActiveState = 0;
        this.MaxActiveState = this.MaxFrame;
        this.HeatCost = 0;
    }

    public RobotHitstunState(int duration) {
        this.MaxFrame = duration;

        this.Initialize();
    }

    public override State HandleInput(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return null;

        RobotStateMachine robotStateMachine = (RobotStateMachine)stateMachine;

        if (!this.IsAnimationPlaying(robotStateMachine, "RobotHitstun")) {
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
    }

    public override void Exit(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return;

        RobotStateMachine robotStateMachine = (RobotStateMachine)stateMachine;

        robotStateMachine.Animator.speed = this.InitialSpeed;
    }

    protected virtual void SetSpeed(RobotStateMachine robotStateMachine) {
        float desiredAnimationTime = this.MaxFrame / (1 / Time.fixedDeltaTime);

        robotStateMachine.Animator.speed = 
            (robotStateMachine.Animator.GetCurrentAnimatorStateInfo(0).length *
            robotStateMachine.Animator.speed) / desiredAnimationTime;

        this.IsSpeedSet = true;
    }
}
