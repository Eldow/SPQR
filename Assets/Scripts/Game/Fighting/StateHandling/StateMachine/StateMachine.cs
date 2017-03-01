using System;
using UnityEngine;

public class StateMachine : Photon.MonoBehaviour {
    [HideInInspector]
    public State CurrentState { get; protected set; }
    [HideInInspector]
    public Automaton Automaton = null;
    protected string NextState = null;
    protected bool IsNewState = false;

    // to be changed in a child class, if necessary
    public virtual string DefaultState {
        get {
            return "State";
        }
    }

    void Update() {
        if (!this.photonView.isMine) return;

        this.HandleInput();
    }

    void FixedUpdate() {
        if (!this.photonView.isMine || this.IsNewState) return;

        if (this.NextState != null) { 
            this.SetState(this.NextState);
        }

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

    protected virtual void SwitchState(State state) {
        if (state == null) return;
        //Debug.Log("SWITCH STATE " + state.GetType().Name + " previous: " + this.CurrentState.GetType().Name);

        Debug.Log("SWITCH STATE ");
        Debug.Log(this.CurrentState);

        Debug.Log(state);

        this.CurrentState.Exit(this);
        this.CurrentState = state;
        this.CurrentState.Enter(this);
        this.NextState = null;
        this.IsNewState = false;
    }

    public virtual void SetState(State state) {
        Debug.Log("SETSTATE AFTER: " + state.GetType().Name);
        this.SwitchState(state);
        Debug.Log("WTF????????????????????????????????????" + state.GetType().Name);
    }

    public virtual void SetState(string stateName) {
        if (stateName == null) return;

        this.IsNewState = true;

        if (!(this is RobotStateMachine)) return;
        Debug.Log("SETSTATE with: " + stateName);

        RobotStateMachine robotStateMachine = (RobotStateMachine)this;
        robotStateMachine.PlayerController.UpdateState(stateName);
    }
}
