public class RobotNoSpecialState : RobotState {
    public override State HandleInput(StateMachine stateMachine) {
        if (InputManager.attackButton()) {
            return new RobotAttack1State();
        }

        return InputManager.blockButton() ? new RobotBlockState() : null;
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
