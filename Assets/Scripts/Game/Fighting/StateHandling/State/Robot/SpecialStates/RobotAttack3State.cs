public class RobotAttack3State : RobotAttackState {
    protected override void Initialize() {
        this.MaxFrame = 40;
        this.IASA = 34;
        this.MinActiveState = 3;
        this.MaxActiveState = 6;
        this.Damage = 3;
        this.Hitstun = 10;
        this.HeatCost = 10;
    }

    public override string HandleInput(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return null;

        RobotStateMachine robotStateMachine = (RobotStateMachine)stateMachine;

        if (!this.IsAnimationPlaying(robotStateMachine, "RobotAttack3")) {
            return null;
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

    public RobotAttack3State() {
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
