using System;
using UnityEngine;

public class StateMachine : MonoBehaviour {
    [HideInInspector]
    public State CurrentState { get; protected set; }
    [HideInInspector]
    public Automaton Automaton = null;
    protected State NextState = null;

    // to be changed in a child class, if necessary
    public virtual string DefaultState {
        get {
            return "State";
        }
    }

    void Update() {
        this.HandleInput();
    }

    void FixedUpdate() {
        this.SwitchState();

        this.CurrentState.Update(this);
    }

    protected virtual void Initialize(string startingState = null) {
        this.Automaton = this.gameObject.GetComponent<Automaton>();
    }

    protected virtual Type CheckStartingState(string startingState) {
        if (String.IsNullOrEmpty(startingState)) {
            startingState = this.DefaultState;
        }

        Type stateType = Type.GetType(startingState);

        if (stateType == null) {
            Debug.LogError(startingState + ": unknown state to initialize!");

            return null;
        }

        return stateType;
    }

    public virtual void HandleInput() {
        this.NextState = this.CurrentState.HandleInput(this);
    }

    protected virtual void SwitchState() {
        if (this.NextState == null) return;

        this.CurrentState.Exit(this);
        this.CurrentState = this.NextState;
        this.CurrentState.Enter(this);
        this.NextState = null;
    }

    public virtual void SetState(State state) {
        this.NextState = state;
        this.SwitchState();
    }

    public virtual void SetState(string stateName)
    {
        if (!(this is RobotStateMachine)) return;
        RobotStateMachine robotStateMachine = (RobotStateMachine)this;
        robotStateMachine.PlayerController.SendStateToMaster(robotStateMachine.PlayerController.ID, stateName);
    }
}
