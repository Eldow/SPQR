using UnityEngine;

public class RobotRunState : RobotState {
    private bool entered;

    public override State HandleInput(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return null;
		
        RobotStateMachine robotStateMachine = (RobotStateMachine) stateMachine;
		
		InputManager inputManager = ((RobotStateMachine) stateMachine).PlayerController.inputManager;
		
        // to be removed when the magic will be working all the time!
        if (inputManager.attackButton()) {
            Debug.Log("Can't attack while running!");
            return null;
        }

        if (inputManager.blockButton()) {
            return new RobotBlockState();
        }

        if (inputManager.dashButton()) {
            return new RobotDashState();
        }

        if (!robotStateMachine.PlayerController.PlayerPhysics.IsRunning()) {
            return new RobotIdleState();
        }

        if (!inputManager.runButton()) {
            return new RobotWalkState();
        }

        /* The animation can be decomposed in three states : startup, running
         * and ending. We have to freeze it in the middle while the player is
         * running.
         */

        if (this.IsCurrentAnimationPlayedPast(robotStateMachine, .5f) &&
            Mathf.Abs(robotStateMachine.Animator.speed) > .01f) {
            this.FreezeAnimation(robotStateMachine);
        }

        return null;
    }

    protected override void Initialize() {
        base.Initialize();

        this.HeatCost = 3;
    }

    public RobotRunState() {
        this.Initialize();
    }

    public override void Update(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return;

        ((RobotStateMachine) stateMachine).PlayerController.PlayerPhysics.Run();
    }

    public override void Enter(StateMachine stateMachine) {
        base.Enter(stateMachine);

        if (!(stateMachine is RobotStateMachine)) return;

        // necessary to keep track of history
        this.SaveToHistory((RobotStateMachine) stateMachine);
        entered = true;
        PlayAudioEffect(((RobotStateMachine) stateMachine).PlayerController.PlayerAudio);
    }

    public override void Exit(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return;

        // the animation don't have to be frozen anymore
        this.ResumeNormalAnimation((RobotStateMachine) stateMachine);
        entered = false;
        PlayAudioEffect(((RobotStateMachine) stateMachine).PlayerController.PlayerAudio);
    }

    public override void PlayAudioEffect(PlayerAudio audio) {
        Debug.Log("PLEIN DE SPEEDUP");
        if (entered) {
            audio.SpeedUp();
        } else {
            //audio.SlowDown();
        }
    }
}
