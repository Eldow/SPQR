public class RobotNoSpecialState : RobotState {
    public override string HandleInput(StateMachine stateMachine) {
        if (InputManager.attackButton()) {
            return typeof(RobotAttack1State).ToString();
        }

        if (InputManager.blockButton()) {
            return typeof(RobotBlockState).Name;
        }

        return null;
    }

    public RobotNoSpecialState() {
        this.Initialize();
    }

    public override void Update(StateMachine stateMachine) {
    }

    public override void Enter(StateMachine stateMachine) {
    }

    public override void Exit(StateMachine stateMachine) {
    }
}
