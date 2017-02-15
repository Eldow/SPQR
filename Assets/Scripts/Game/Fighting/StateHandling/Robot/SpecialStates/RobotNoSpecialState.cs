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

        if (Input.GetKeyDown("joystick button 1")) {
            return new RobotBlockState();
        }

        return null;
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
