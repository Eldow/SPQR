using UnityEngine;

public class RobotRunState : RobotState {
    public override RobotState HandleInput(RobotStateMachine stateMachine, 
        XboxInput xboxInput) {
        // to be removed when the magic will be working all the time!
        if (Input.GetKeyDown(xboxInput.A)) {
            Debug.Log("Can't attack while running!");
            return null;
        }

        if (Input.GetKeyDown(xboxInput.B)) {
            stateMachine.Animator.SetBool("IsRun", false);

            return new RobotBlockState();
        }

        if (Mathf.Abs(xboxInput.getLeftStickX()) <= 0.2f &&
            Mathf.Abs(xboxInput.getLeftStickY()) <= 0.2f) {
            stateMachine.Animator.SetBool("IsRun", false);

            return new RobotIdleState();
        }

        if (!xboxInput.RT()) {
            stateMachine.Animator.SetBool("IsRun", false);

            return new RobotWalkState();
        }

        /* The animation can be decomposed in three states : startup, running
         * and ending. We have to freeze it in the middle while the player is
         * running.
         */
        if (this.IsCurrentAnimationPlayedPast(stateMachine, .5f) &&
            Mathf.Abs(stateMachine.Animator.speed) > .01f) {
            this.FreezeAnimation(stateMachine);
        }

        return null;
    }

    public override void Update(RobotStateMachine stateMachine) {
        stateMachine.PlayerController.RunMovement(); // movement is allowed
    }

    public override void Enter(RobotStateMachine stateMachine) {
        Debug.Log("RUN ENTER!");
        stateMachine.Animator.SetBool("IsRun", true); // always necessary!
        this.SaveToHistory(stateMachine); // necessary to keep track of history
    }

    public override void Exit(RobotStateMachine stateMachine) {
        Debug.Log("RUN EXIT!");
        // the animation don't have to be frozen anymore
        this.ResumeAnimation(stateMachine);
    }
}
