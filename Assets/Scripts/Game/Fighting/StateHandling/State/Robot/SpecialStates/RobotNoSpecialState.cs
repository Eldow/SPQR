using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotNoSpecialState : RobotState {
    public override RobotState HandleInput(
    RobotStateMachine stateMachine, XboxInput xboxInput) {
        if (Input.GetKeyDown(xboxInput.A)) {
            return new RobotAttackState();
        }

        return Input.GetKeyDown(xboxInput.B) ? new RobotBlockState() : null;
    }

    public override void Update(RobotStateMachine stateMachine) {
    }

    public override void Enter(RobotStateMachine stateMachine) {
        Debug.Log("NOSPECIALSTATE ENTER!");
    }

    public override void Exit(RobotStateMachine stateMachine) {
        Debug.Log("NOSPECIALSTATE EXIT!");
    }
}
