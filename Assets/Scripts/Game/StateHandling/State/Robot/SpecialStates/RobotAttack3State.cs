public class RobotAttack3State : RobotAttackState {
    protected override void Initialize() {
        this.MaxFrame = 40;
        this.IASA = 34;
        this.MinActiveState = 8;
        this.MaxActiveState = 40;
        this.Damage = 5;
        this.Hitstun = 30;
        this.HeatCost = 10;
    }

    public override State HandleInput(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return null;

        RobotStateMachine robotStateMachine = (RobotStateMachine)stateMachine;
		
		InputManager inputManager = ((RobotStateMachine) stateMachine).PlayerController.inputManager;

        if (!this.IsAnimationPlaying(robotStateMachine, "RobotAttack3")) {
            return null;
        }

        if (this.IsInterruptible(robotStateMachine)) { // can be interrupted!
            RobotState newState = this.CheckInterruptibleActions(stateMachine);

            if (newState != null) return newState;
        }

        if (this.IsStateFinished()) {
            if (this.IsLastState(robotStateMachine, "RobotWalkState")) {
                return new RobotWalkState();
            }

            return new RobotIdleState();
        }

        return null;
    }

    public RobotAttack3State() {
        this.Initialize();
    }

    public override void Update(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return;

        this.CurrentFrame++;
		alreadyHitByAttack = false;
        ((RobotStateMachine)stateMachine).PlayerController.PlayerPhysics
            .Move();
    }

    public override void Exit(StateMachine stateMachine) {
    }

    public override RobotState CheckInterruptibleActions(StateMachine stateMachine) {
		InputManager inputManager = ((RobotStateMachine) stateMachine).PlayerController.inputManager;
        if (inputManager.moveX() > .02f || inputManager.moveY() > .02f) {
            if (inputManager.runButton()) {
                return new RobotRunState();
            }

            return new RobotWalkState();
        }

        return null;
    }

    public override void PlayAudioEffect(PlayerAudio audio)
    {
        audio.Whirlwind();
    }
}
