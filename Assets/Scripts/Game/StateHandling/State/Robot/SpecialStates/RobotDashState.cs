public class RobotDashState : RobotFramedState {
    protected override void Initialize() {
        this.MaxFrame = 16;
        this.IASA = 8;
        this.MinActiveState = 6;
        this.MaxActiveState = 23;
        this.HeatCost = 18;
    }

    public override State HandleInput(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return null;

        RobotStateMachine robotStateMachine = (RobotStateMachine) stateMachine;
		
		InputManager inputManager = ((RobotStateMachine) stateMachine).PlayerController.inputManager;

        this.ResumeNormalAnimation(robotStateMachine);

        if (this.IsStateFinished()) {
            if (this.IsLastState(robotStateMachine, "RobotWalkState")) {
                return new RobotWalkState();
            }
            if (this.IsLastState(robotStateMachine, "RobotRunState")) {
                return new RobotRunState();
            }

            return new RobotIdleState();
        }

        if (inputManager.attackButton()) {
            return new RobotAttack1State();
        }

        /*
        if (this.IsInterruptible(robotStateMachine))
        { // can be interrupted!
            RobotState newState = this.CheckInterruptibleActions();

            if (newState != null) return newState;
        }*/

        return null;
    }

    public RobotDashState() {
        this.Initialize();
    }

    public override void Update(StateMachine stateMachine) {
        this.CurrentFrame++;
        if (!(stateMachine is RobotStateMachine)) return;
        RobotStateMachine robotStateMachine = (RobotStateMachine) stateMachine;
        robotStateMachine.PlayerController.PlayerPhysics.Dash();
    }

    public override void Exit(StateMachine stateMachine) {}

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

    public override void PlayAudioEffect(PlayerAudio audio) {
        audio.Dash();
    }
}