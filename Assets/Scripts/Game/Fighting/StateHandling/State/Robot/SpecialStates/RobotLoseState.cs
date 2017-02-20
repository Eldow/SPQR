using UnityEngine;

public class RobotLoseState : RobotState {

    public override State HandleInput(StateMachine stateMachine) {
        return null;
    }

    public override void Update(StateMachine stateMachine) {

    }

    public override void Enter(StateMachine stateMachine) {
        Debug.Log("LOSE ENTER!");
    }

    public override void Exit(StateMachine stateMachine) {
        Debug.Log("LOSE EXIT!");
    }
}
