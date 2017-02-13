using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotWalkState : RobotState {
    void Start() {

    }

    void Update() {

    }

    public override RobotState HandleInput(
    RobotStateMachine stateMachine, XboxInput xboxInput) {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) <= 0.2f &&
            Mathf.Abs(Input.GetAxis("Vertical")) <= 0.2f) {
            return new RobotIdleState();
        } else {
            return null;
        }
    }

    public override void Update(RobotStateMachine stateMachine) {
        stateMachine.PlayerController.UnlockedMovement();
    }

    public override void Enter(RobotStateMachine stateMachine) {
        stateMachine.Animator.SetBool("IsWalk", true);
    }

    public override void Exit(RobotStateMachine stateMachine) {
        stateMachine.Animator.SetBool("IsWalk", false);
    }
}
