using UnityEngine;

public class RobotBlockState : RobotFramedState {
    protected override void Initialize() {
        this.MaxFrame = 32;
        this.IASA = 8;
        this.MinActiveState = 6;
        this.MaxActiveState = 23;
        this.HeatCost = 3;
    }

    public override string HandleInput(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return null;

        RobotStateMachine robotStateMachine = (RobotStateMachine)stateMachine;

        if (!this.IsAnimationPlaying(robotStateMachine, "RobotBlock")) {
            return null;
        }

        if (this.CheckIfBlockHolding()) {
            if (this.IsCurrentAnimationPlayedPast(robotStateMachine, .5f) && 
                Mathf.Abs(robotStateMachine.Animator.speed) > .01f) {
                this.FreezeAnimation(robotStateMachine);
            }

            return null;
        }

        this.ResumeAnimation(robotStateMachine);

        if (this.IsInterruptible(robotStateMachine)) { // can be interrupted!
            string newState = this.CheckInterruptibleActions();

            if (newState != null) return newState;
        }

        if (this.IsStateFinished()) {
            return typeof(RobotIdleState).Name;
        }

        return null;
    }

    public RobotBlockState() {
        this.Initialize();
    }

    public override void Update(StateMachine stateMachine) {
        if (this.CheckIfBlockHolding()) return;

        this.CurrentFrame++;
    }

    public override void Exit(StateMachine stateMachine) {
    }

    protected virtual bool CheckIfBlockHolding() {
        return InputManager.blockButton();
    }

    public override string CheckInterruptibleActions() {
        if (InputManager.moveX() > .02f || InputManager.moveY() > .02f) {
            if (InputManager.runButton()) {
                return typeof(RobotRunState).Name;
            }

            return typeof(RobotWalkState).Name;
        }

        return null;
    }
}
