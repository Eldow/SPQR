using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.NetworkSystem;

public class RobotStateMachine : StateMachine {
    public Animator Animator = null;
    public PlayerController PlayerController = null;
    protected RobotState _robotState = null;

    public virtual String DefaultState {
        get {
            return "RobotIdleState";
        }
    }

    void Start() {
        this.Initialize();
    }

    void Update() {
        this.HandleInput(this.PlayerController.xboxInput);
        this._robotState.Update(this);
    }

    public virtual void Initialize(String startingState = null) {
        this.Animator = this.GetComponent<Animator>();
        this.PlayerController = this.GetComponent<PlayerController>();

        if (String.IsNullOrEmpty(startingState)) {
            startingState = this.DefaultState;
        }

        Type stateType = Type.GetType(startingState);

        if (stateType == null) {
            Debug.LogError(startingState + ": unknown state to initialize!");
            this._robotState = new RobotState(); // for logic's sake
        } else {
            this._robotState = (RobotState)Activator.CreateInstance(stateType);
        }        
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
