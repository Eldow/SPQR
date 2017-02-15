using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBlockState : RobotState {
    void Start() {

    }

    void Update() {

    }

    public override RobotState HandleInput(
    RobotStateMachine stateMachine, XboxInput xboxInput) {
        if (!stateMachine.Animator.GetCurrentAnimatorStateInfo(0)
            .IsName("RobotBlock")) {
            return null;
        }

        if (stateMachine.Animator.GetCurrentAnimatorStateInfo(0)
            .normalizedTime > 1 &&
            !stateMachine.Animator.IsInTransition(0)) {
            return new RobotIdleState();
        } else {
            return null;
        }
    }

    public override void Update(RobotStateMachine stateMachine) {

    }

    public override void Enter(RobotStateMachine stateMachine) {
        Debug.Log("BLOCK ENTER!");
        stateMachine.Animator.SetBool("IsBlock", true);
    }

    public override void Exit(RobotStateMachine stateMachine) {
        Debug.Log("BLOCK EXIT!");
        stateMachine.Animator.SetBool("IsBlock", false);
    }
}
