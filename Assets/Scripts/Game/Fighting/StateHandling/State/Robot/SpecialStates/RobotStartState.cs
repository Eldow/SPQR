public class RobotStartState : RobotState {

    public override string HandleInput(StateMachine stateMachine) {
        return null;
    }

    public RobotStartState() {
        this.Initialize();
    }

    public override void Update(StateMachine stateMachine) {

    }

    public override void Enter(StateMachine stateMachine) {
    }

    public override void Exit(StateMachine stateMachine) {
    }
}
