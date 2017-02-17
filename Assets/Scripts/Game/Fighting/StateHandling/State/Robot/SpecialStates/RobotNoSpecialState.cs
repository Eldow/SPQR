using UnityEngine;

public class RobotNoSpecialState : RobotState {
    public override State HandleInput(StateMachine stateMachine,
        XboxInput xboxInput) {
        if (Input.GetKeyDown(xboxInput.A)) {
            return new RobotAttack1State();
        }

        return Input.GetKeyDown(xboxInput.B) ? new RobotBlockState() : null;
    }

    public override void Update(StateMachine stateMachine) {
    }

    public override void Enter(StateMachine stateMachine) {
        Debug.Log("NOSPECIALSTATE ENTER!");
    }

    public override void Exit(StateMachine stateMachine) {
        Debug.Log("NOSPECIALSTATE EXIT!");
    }
}
