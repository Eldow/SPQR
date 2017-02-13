using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotIdleState : RobotState {
    void Start() {

    }

    void Update() {

    }

    public override RobotState HandleInput(
    RobotStateMachine stateMachine, XboxInput xboxInput) {
        string x = Input.GetAxis("Horizontal").ToString();
        string y = Input.GetAxis("Vertical").ToString();
        string test = "X " + x + " | " + "Y " + y;
        Debug.Log(test);
        if (Mathf.Abs(Input.GetAxis("Horizontal")) <= 0.2f &&
            Mathf.Abs(Input.GetAxis("Vertical")) <= 0.02) {
            return null;
        } else {
            return new RobotWalkState();
        }
    }

    public override void Update(RobotStateMachine stateMachine) {

    }

    public override void Enter(RobotStateMachine stateMachine) {

    }

    public override void Exit(RobotStateMachine stateMachine) {

    }
}
