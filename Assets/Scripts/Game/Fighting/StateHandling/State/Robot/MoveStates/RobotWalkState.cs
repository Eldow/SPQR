using UnityEngine;

public class RobotWalkState : RobotState {
    public override State HandleInput(StateMachine stateMachine,
        XboxInput xboxInput) {
        if (!(stateMachine is RobotStateMachine)) return null;
        if (Input.GetKeyDown(xboxInput.A)) {
            return new RobotAttack1State();
        }

        if (Input.GetKeyDown(xboxInput.B)) {
            return new RobotBlockState();
        }

        if (Mathf.Abs(xboxInput.getLeftStickX()) <= 0.2f &&
            Mathf.Abs(xboxInput.getLeftStickY()) <= 0.2f) {
            return new RobotIdleState();
        }

        if (xboxInput.RT()) {
            return new RobotRunState();
        }


        /* The animation can be decomposed in three states : startup, walking
         * and ending. We have to freeze it in the middle while the player is
         * walking.
         */
        RobotStateMachine robotStateMachine = (RobotStateMachine)stateMachine;

        if (this.IsCurrentAnimationPlayedPast(robotStateMachine, .5f) &&
            Mathf.Abs(robotStateMachine.Animator.speed) > .01f) {
            this.FreezeAnimation(robotStateMachine);
        }

        return null;
    }

    public override void Update(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return;

        ((RobotStateMachine)stateMachine).PlayerController.Movement();
    }

    public override void Enter(StateMachine stateMachine) {
        Debug.Log("WALK ENTER!");
        if (!(stateMachine is RobotStateMachine)) return;

        // necessary to keep track of history
        this.SaveToHistory((RobotStateMachine)stateMachine);
    }

    public override void Exit(StateMachine stateMachine) {
        Debug.Log("WALK EXIT!");

        if (!(stateMachine is RobotStateMachine)) return;

        // the animation don't have to be frozen anymore
        this.ResumeAnimation((RobotStateMachine)stateMachine);
    }
}
