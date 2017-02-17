using System;
using System.Collections.Generic;
using UnityEngine;

public class RobotStateMachine : StateMachine {
    public Animator Animator = null;
    public PlayerController PlayerController = null;
    protected RobotState RobotState = null;
    [HideInInspector]
    public RobotAutomaton RobotAutomata = null;
    [HideInInspector] public FixedSizedQueue<string> StateHistory;
    public int MaxHistorySize = 12;

    // to be changed in a child class, if necessary
    public override string DefaultState {
        get {
            return "RobotIdleState";
        }
    }

    void Start() {
        this.Initialize();
    }

    void FixedUpdate() {
        this.HandleInput(this.PlayerController.xboxInput);
        this.RobotState.Update(this);
    }

    protected override void Initialize(string startingState = null) {
        this.Animator = this.GetComponent<Animator>();
        this.PlayerController = this.GetComponent<PlayerController>();

        if (String.IsNullOrEmpty(startingState)) {
            startingState = this.DefaultState;
        }

        Type stateType = this.CheckStartingState(startingState);

        if (stateType == null) return;

        this.RobotState = (RobotState)Activator.CreateInstance(stateType);

        this.RobotAutomata = this.gameObject.GetComponent<RobotAutomaton>();
        this.StateHistory = new FixedSizedQueue<string>(this.MaxHistorySize);
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

        this.Animator.SetTrigger(robotState.GetType().Name);
    }

    public virtual void SetState(RobotState robotState) {
        this.SwitchState(robotState);
    }
}
