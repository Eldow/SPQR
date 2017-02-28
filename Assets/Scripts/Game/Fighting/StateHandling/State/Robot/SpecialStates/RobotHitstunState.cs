using UnityEngine;

public class RobotHitstunState : RobotState {

    private float HitstunTime;
    private float StunStartTime;
    private bool IsStunOver;

    public RobotHitstunState(float Duration) {
        StunStartTime = Time.frameCount;
        HitstunTime = Duration;
    }

    public RobotHitstunState() {
        this.Initialize();
    }

    public override State HandleInput(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return null;

        RobotStateMachine robotStateMachine = (RobotStateMachine)stateMachine;

        if (!this.IsAnimationPlaying(robotStateMachine, "RobotHitstun")) {
            return null;
        }

        if (this.IsStunOver) {
            return new RobotWalkState();
        }

        return null;
    }

    public override void Update(StateMachine stateMachine) {
        IsStunOver =  ((Time.frameCount - StunStartTime) >= HitstunTime);
    }

    public override void Enter(StateMachine stateMachine) {
    }

    public override void Exit(StateMachine stateMachine) {
    }
}
