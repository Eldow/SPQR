using System;

public class RootSceneStateMachine : StateMachine {
    // to be changed in a child class, if necessary
    public virtual string DefaultState {
        get {
            return "State";
        }
    }

    protected override void Initialize(string startingState = null) {
        Type stateType = this.CheckStartingState(startingState);

        if (stateType == null) return;

        // code
    }
}
