using UnityEngine;

public class RobotRunState : RobotState {
    public override State HandleInput(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return null;

        // to be removed when the magic will be working all the time!
		if (InputManager.attackButton()) {
            Debug.Log("Can't attack while running!");
            return null;
        }

		if (InputManager.blockButton()) {
            return new RobotBlockState();
        }

		if (Mathf.Abs(InputManager.moveX()) <= 0.2f &&
			Mathf.Abs(InputManager.moveY()) <= 0.2f) {
            return new RobotIdleState();
        }

		if (!InputManager.runButton()) {
            return new RobotWalkState();
        }

        /* The animation can be decomposed in three states : startup, running
         * and ending. We have to freeze it in the middle while the player is
         * running.
         */
        RobotStateMachine robotStateMachine = (RobotStateMachine) stateMachine;

        if (this.IsCurrentAnimationPlayedPast(robotStateMachine, .5f) &&
            Mathf.Abs(robotStateMachine.Animator.speed) > .01f) {
            this.FreezeAnimation(robotStateMachine);
        }

        return null;
    }

    public override void Update(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return;

        ((RobotStateMachine)stateMachine).PlayerController.PlayerPhysics
            .RunMovement();
    }

    public override void Enter(StateMachine stateMachine) {
        Debug.Log("RUN ENTER!");
        if (!(stateMachine is RobotStateMachine)) return;

        // necessary to keep track of history
        this.SaveToHistory((RobotStateMachine) stateMachine);
    }

    public override void Exit(StateMachine stateMachine) {
        Debug.Log("RUN EXIT!");

        if (!(stateMachine is RobotStateMachine)) return;

        // the animation don't have to be frozen anymore
        this.ResumeAnimation((RobotStateMachine) stateMachine);
    }
}
