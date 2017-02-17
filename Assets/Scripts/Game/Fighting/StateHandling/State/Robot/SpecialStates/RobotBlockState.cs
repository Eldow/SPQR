using UnityEngine;

public class RobotBlockState : RobotState {
    protected override void Initialize() {
        this.IASA = .7f;
    }

    public override State HandleInput(StateMachine stateMachine,
        XboxInput xboxInput) {
        if (!(stateMachine is RobotStateMachine)) return null;

        RobotStateMachine robotStateMachine = (RobotStateMachine)stateMachine;

        if (!this.IsAnimationPlaying(robotStateMachine, "RobotBlock")) {
            return null;
        }

        if (this.CheckIfBlockHolding(xboxInput)) {
            if (this.IsCurrentAnimationPlayedPast(robotStateMachine, .5f) && 
                Mathf.Abs(robotStateMachine.Animator.speed) > .01f) {
                this.FreezeAnimation(robotStateMachine);
            }

            return null;
        }

        this.ResumeAnimation(robotStateMachine);

        if (this.IsInterruptible(robotStateMachine) && // can be interrupted!
            (xboxInput.getLeftStickX() > .02f || 
            xboxInput.getLeftStickY() > .02f)) {
            if (xboxInput.RT()) {
                return new RobotRunState();
            }

            return new RobotWalkState();
        }

        if (this.IsCurrentAnimationFinished(robotStateMachine)) {

            return new RobotIdleState();
        }

        return null;
    }

    public override void Update(StateMachine stateMachine) {

    }

    public override void Enter(StateMachine stateMachine) {
        Debug.Log("BLOCK ENTER!");
    }

    public override void Exit(StateMachine stateMachine) {
        Debug.Log("BLOCK EXIT!");
    }

    protected virtual bool CheckIfBlockHolding(XboxInput xboxInput) {
        return Input.GetKey(xboxInput.B);
    }
}
