using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotStateMachine : StateMachine {
    [HideInInspector] public Animator Animator = null;
    [HideInInspector] public PlayerController PlayerController = null;
    public String DefaultState = "RobotIdleState";
    private RobotState _robotState = null;

    void Start() {
        this.Animator = this.GetComponent<Animator>();
        this.PlayerController = this.GetComponent<PlayerController>();

        if (this._robotState == null) {
            //this._state = new RobotIdleState();
            Type stateType = Type.GetType(this.DefaultState);
            this._robotState = (RobotState)Activator.CreateInstance(stateType);
        }
    }

    void Update() {
        this.HandleInput(this.PlayerController.xboxInput);
        this._robotState.Update(this);
    }    

    public override void HandleInput(XboxInput xboxInput) {
        RobotState robotState = this._robotState.HandleInput(this, xboxInput);

        if (robotState != null) {
            this._robotState.Exit(this);
            this._robotState = robotState;
            this._robotState.Enter(this);
        }
    }
}
