using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotNoSpecialState : RobotState {
    void Start() {

    }

    void Update() {

    }

    public override RobotState HandleInput(
    RobotStateMachine stateMachine, XboxInput xboxInput) {
        if (Input.GetKeyDown("joystick button 0")) {
            return new RobotAttackState();
        }

        return null;
    }

    public override void Update(RobotStateMachine stateMachine) {
    }

    public override void Enter(RobotStateMachine stateMachine) {
    }

    public override void Exit(RobotStateMachine stateMachine) {
    }
}
