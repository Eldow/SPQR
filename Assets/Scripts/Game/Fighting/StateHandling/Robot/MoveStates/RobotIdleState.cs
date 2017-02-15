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
        if (!stateMachine.Animator.GetCurrentAnimatorStateInfo(0)
            .IsName("RobotIdle")) {
            return null;
        }

        if (Input.GetKeyDown("joystick button 0")) {
            return new RobotAttackState();
        }

        if (Input.GetKeyDown("joystick button 1")) {
            return new RobotBlockState();
        }

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
        Debug.Log("IDLE ENTER!");
        stateMachine.Animator.SetBool("IsWalk", false);
    }

    public override void Exit(RobotStateMachine stateMachine) {
        Debug.Log("IDLE EXIT!");
    }
}
