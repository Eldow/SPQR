using System;
using UnityEngine;

public class StateMachine : MonoBehaviour {
    [HideInInspector]
    public State CurrentState { get; protected set; }
    [HideInInspector]
    public Automaton Automaton = null;

    // to be changed in a child class, if necessary
    public virtual string DefaultState {
        get {
            return "State";
        }
    }

    void FixedUpdate() {
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
        State state = this.CurrentState.HandleInput(this);
	
        this.SwitchState(state);
    }

    protected virtual void SwitchState(State state) {
        if (state == null) return;

        this.CurrentState.Exit(this);
        this.CurrentState = state;
        this.CurrentState.Enter(this);
    }

    public virtual void SetState(State state) {
        this.SwitchState(state);
    }
}
