using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotState : State {
    void Start() {

    }

    void Update() {
       
    }

    public virtual RobotState HandleInput(
    RobotStateMachine stateMachine, XboxInput xboxInput) {
        return null;
    }

    public virtual void Update(RobotStateMachine stateMachine) {

    }

    public virtual void Enter(RobotStateMachine stateMachine) {

    }

    public virtual void Exit(RobotStateMachine stateMachine) {

    }
}
