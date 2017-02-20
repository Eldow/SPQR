using UnityEngine;

public class RobotWinState : RobotState {

    public override State HandleInput(StateMachine stateMachine) {
        return null;
    }

    public override void Update(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return;
        ((RobotStateMachine)stateMachine).PlayerController.Movement();
    }

    public override void Enter(StateMachine stateMachine) {
        Debug.Log("WIN ENTER!");
    }

    public override void Exit(StateMachine stateMachine) {
        Debug.Log("WIN EXIT!");
    }
}
