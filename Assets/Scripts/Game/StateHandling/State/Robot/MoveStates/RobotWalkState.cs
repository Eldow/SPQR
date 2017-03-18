using UnityEngine;

public class RobotWalkState : RobotState {
    public override State HandleInput(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return null;
        if (InputManager.attackButton()) {
            return new RobotAttack1State();
        }

        if (InputManager.blockButton()) {
            return new RobotBlockState();
        }

        if (InputManager.dashButton()) {
            return new RobotDashState();
        }

        if (InputManager.powerAttackButton()) {
            return new RobotPowerAttackState();
        }

        if (!((RobotStateMachine) stateMachine).PlayerController.PlayerPhysics
            .IsWalking()) {
            return new RobotIdleState();
        }

        if (InputManager.runButton()) {
            return new RobotRunState();
        }


        /* The animation can be decomposed in three states : startup, walking
         * and ending. We have to freeze it in the middle while the player is
         * walking.
         */
        RobotStateMachine robotStateMachine = (RobotStateMachine) stateMachine;

        if (this.IsCurrentAnimationPlayedPast(robotStateMachine, .5f) &&
            Mathf.Abs(robotStateMachine.Animator.speed) > .01f) {
            this.FreezeAnimation(robotStateMachine);
        }

        return null;
    }

    public RobotWalkState() {
        this.Initialize();
    }

    public override void Update(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return;

        ((RobotStateMachine) stateMachine).PlayerController.PlayerPhysics
            .Move();
    }

    public override void Enter(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return;

        // necessary to keep track of history
        this.SaveToHistory((RobotStateMachine) stateMachine);
    }

    public override void Exit(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return;

        // the animation don't have to be frozen anymore
        this.ResumeAnimation((RobotStateMachine) stateMachine);
    }
}
