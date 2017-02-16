using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.NetworkSystem;

public class RobotStateMachine : StateMachine {
    public Animator Animator = null;
    public PlayerController PlayerController = null;
    protected RobotState RobotState = null;
    [HideInInspector]
    public RobotAutomaton RobotAutomata = null;

    // to be changed in a child class, if necessary
    public virtual string DefaultState {
        get {
            return "RobotIdleState";
        }
    }

    void Start() {
        this.Initialize();
    }

    void Update() {
        this.HandleInput(this.PlayerController.xboxInput);
        this.RobotState.Update(this);
    }

    public virtual void Initialize(string startingState = null) {
        this.Animator = this.GetComponent<Animator>();
        this.PlayerController = this.GetComponent<PlayerController>();

        if (String.IsNullOrEmpty(startingState)) {
            startingState = this.DefaultState;
        }

        Type stateType = Type.GetType(startingState);

        if (stateType == null) {
            Debug.LogError(startingState + ": unknown state to initialize!");
            this.RobotState = new RobotState(); // for logic's sake
        } else {
            this.RobotState = (RobotState)Activator.CreateInstance(stateType);
        }

        this.RobotAutomata = this.gameObject.GetComponent<RobotAutomaton>();
    }

    public override void HandleInput(XboxInput xboxInput) {
        RobotState robotState = this.RobotState.HandleInput(this, xboxInput);

        this.SwitchState(robotState);
    }

    protected virtual void SwitchState(RobotState robotState) {
        if (robotState == null) return;

        this.RobotState.Exit(this);
        this.RobotState = robotState;
        this.RobotState.Enter(this);
    }

    public virtual void SetState(RobotState robotState) {
        this.SwitchState(robotState);
    }
}
