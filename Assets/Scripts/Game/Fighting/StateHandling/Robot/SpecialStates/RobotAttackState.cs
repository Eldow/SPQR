using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAttackState : RobotState {
    void Start() {

    }

    void Update() {

    }

    public override RobotState HandleInput(
    RobotStateMachine stateMachine, XboxInput xboxInput) {
        if (!stateMachine.Animator.GetCurrentAnimatorStateInfo(0)
            .IsName("RobotAttack")) {
            return null;
        }

        if (stateMachine.Animator.GetCurrentAnimatorStateInfo(0)
            .normalizedTime > 1 &&
            !stateMachine.Animator.IsInTransition(0)) {
            if (stateMachine.Animator.GetBool("IsWalk")) {
                return new RobotWalkState();
            } else {
                return new RobotIdleState();
            }
        } else {
            return null;
        }
    }

    public override void Update(RobotStateMachine stateMachine) {
        stateMachine.PlayerController.UnlockedMovement();
    }

    public override void Enter(RobotStateMachine stateMachine) {
        Debug.Log("ATTACK ENTER!");
        stateMachine.Animator.SetBool("IsAttack", true);
    }

    public override void Exit(RobotStateMachine stateMachine) {
        Debug.Log("ATTACK EXIT!");
        stateMachine.Animator.SetBool("IsAttack", false);
    }
}
