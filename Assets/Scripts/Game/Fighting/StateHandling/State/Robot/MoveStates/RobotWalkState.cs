using UnityEngine;

public class RobotWalkState : RobotState {
    public override RobotState HandleInput(RobotStateMachine stateMachine, 
        XboxInput xboxInput) {
        if (Input.GetKeyDown(xboxInput.A)) { // IsWalk is still true!

            return new RobotAttackState();
        }

        if (Input.GetKeyDown(xboxInput.B)) {
            // prevent from going back to Idle Animation
            stateMachine.Animator.SetBool("IsBlock", true);
            stateMachine.Animator.SetBool("IsWalk", false);

            return new RobotBlockState();
        }

        if (Mathf.Abs(xboxInput.getLeftStickX()) <= 0.2f &&
            Mathf.Abs(xboxInput.getLeftStickY()) <= 0.2f) {
            stateMachine.Animator.SetBool("IsWalk", false);

            return new RobotIdleState();
        }

        if (xboxInput.RT()) {
            stateMachine.Animator.SetBool("IsWalk", false);

            return new RobotRunState();
        }


        /* The animation can be decomposed in three states : startup, walking
         * and ending. We have to freeze it in the middle while the player is
         * walking.
         */
        if (this.IsCurrentAnimationPlayedPast(stateMachine, .5f) &&
            Mathf.Abs(stateMachine.Animator.speed) > .01f) {
            this.FreezeAnimation(stateMachine);
        }

        return null;
    }

    public override void Update(RobotStateMachine stateMachine) {
        stateMachine.PlayerController.Movement(); // movement is allowed
    }

    public override void Enter(RobotStateMachine stateMachine) {
        Debug.Log("WALK ENTER!");
        stateMachine.Animator.SetBool("IsWalk", true); // always necessary!
        this.SaveToHistory(stateMachine); // necessary to keep track of history
    }

    public override void Exit(RobotStateMachine stateMachine) {
        Debug.Log("WALK EXIT!");
        // the animation don't have to be frozen anymore
        this.ResumeAnimation(stateMachine);
    }
}
