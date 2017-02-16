using UnityEngine;

public class RobotBlockState : RobotState {
    public override RobotState HandleInput(RobotStateMachine stateMachine, 
        XboxInput xboxInput) {
        if (!this.IsAnimationPlaying(stateMachine, "RobotBlock")) {
            return null;
        }

        if (this.IsCurrentAnimationFinished(stateMachine)) {
            return new RobotIdleState();
        } else {
            return null;
        }
    }

    public override void Update(RobotStateMachine stateMachine) {

    }

    public override void Enter(RobotStateMachine stateMachine) {
        Debug.Log("BLOCK ENTER!");
        stateMachine.Animator.SetBool("IsBlock", true);
    }

    public override void Exit(RobotStateMachine stateMachine) {
        Debug.Log("BLOCK EXIT!");
        stateMachine.Animator.SetBool("IsBlock", false);
    }
}
