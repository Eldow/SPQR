using UnityEngine;

public class RobotBlockState : RobotState {
    protected override void Initialize() {
        this.IASA = .7f;
    }

    public override RobotState HandleInput(RobotStateMachine stateMachine, 
        XboxInput xboxInput) {
        if (!this.IsAnimationPlaying(stateMachine, "RobotBlock")) {
            return null;
        }

        if (this.CheckIfBlockHolding(xboxInput)) {
            if (this.IsCurrentAnimationPlayedPast(stateMachine, .5f) && 
                Mathf.Abs(stateMachine.Animator.speed) > .01f) {
                this.FreezeAnimation(stateMachine);
            }

            return null;
        }

        this.ResumeAnimation(stateMachine);

        if (this.IsInterruptible(stateMachine) && // can be interrupted!
            (xboxInput.getLeftStickX() > .02f || 
            xboxInput.getLeftStickY() > .02f)) {
            if (xboxInput.RT()) {
                return new RobotRunState();
            }

            return new RobotWalkState();
        }

        if (this.IsCurrentAnimationFinished(stateMachine)) {
            return new RobotIdleState();
        }

        return null;
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

    protected virtual bool CheckIfBlockHolding(XboxInput xboxInput) {
        return Input.GetKey(xboxInput.B);
    }
}
