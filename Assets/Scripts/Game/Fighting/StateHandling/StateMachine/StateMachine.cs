using System;
using UnityEngine;

public class StateMachine : MonoBehaviour {
    // to be changed in a child class, if necessary
    public virtual string DefaultState {
        get {
            return "State";
        }
    }

    protected virtual void Initialize(string startingState = null) {
        
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

    public virtual void HandleInput(XboxInput xboxInput) {

    }
}
