using UnityEngine;

public class RobotHitstunState : RobotState {

    private float HitstunTime;
    private float StunStartTime;
    private bool IsStunOver;

    public RobotHitstunState(float Duration) {
        StunStartTime = Time.frameCount;
        HitstunTime = Duration;
    }

    public override State HandleInput(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return null;

        RobotStateMachine robotStateMachine = (RobotStateMachine)stateMachine;

        if (!this.IsAnimationPlaying(robotStateMachine, "RobotHitstun")) {
            return null;
        }

        /*if (this.IsCurrentAnimationFinished(robotStateMachine)) {
            if (this.IsLastState(robotStateMachine, "RobotWalkState")) {
                return new RobotWalkState();
            }

            return new RobotIdleState();
        }*/

        if (IsStunOver) {
            return new RobotWalkState();
        }
        else return null;
    }

    public override void Update(StateMachine stateMachine) {
        IsStunOver =  ((Time.frameCount - StunStartTime) >= HitstunTime);
    }

    public override void Enter(StateMachine stateMachine) {
    }

    public override void Exit(StateMachine stateMachine) {
    }
}
