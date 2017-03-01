using UnityEngine;
public class RobotHitstunState : RobotFramedState {
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
    }

    public override void Exit(StateMachine stateMachine) {
    }
}
