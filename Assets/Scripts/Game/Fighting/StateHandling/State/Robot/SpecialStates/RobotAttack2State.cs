public class RobotAttack2State : RobotAttackState {
    protected override void Initialize() {
        this.MaxFrame = 30;
        this.IASA = 21;
        this.MinActiveState = 7;
        this.MaxActiveState = 13;
        this.Damage = 1;
        this.Hitstun = 5;
        this.HeatCost = 3;
    }

    public override string HandleInput(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return null;

        RobotStateMachine robotStateMachine = (RobotStateMachine)stateMachine;

        if (!this.IsAnimationPlaying(robotStateMachine, "RobotAttack2")) {
            return null;
        }

		if (InputManager.attackButton()) {
            return typeof(RobotAttack3State).Name;
        }

        if (this.IsInterruptible(robotStateMachine)) { // can be interrupted!
            string newState = this.CheckInterruptibleActions();

            if (newState != null) return newState;
        }

        if (this.IsStateFinished()) {
            if (this.IsLastState(robotStateMachine, "RobotWalkState")) {
                return typeof(RobotWalkState).Name;
            }

            return typeof(RobotIdleState).Name;
        }

        return null;
    }

    public RobotAttack2State() {
        this.Initialize();
    }

    public override void Update(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return;

        this.CurrentFrame++;

        ((RobotStateMachine)stateMachine).PlayerController.PlayerPhysics
            .Move();
    }

    public override void Exit(StateMachine stateMachine) {
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
