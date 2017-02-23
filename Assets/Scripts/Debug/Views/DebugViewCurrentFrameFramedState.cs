using UnityEngine;

public class DebugViewCurrentFrameFramedState : DebugViewCurrentState {
    public RobotFramedState RobotFramedState = null;

    // to be overriden in child, if necessary
    public override string Label {
        get {
            return "FS: ";
        }
    }

    void Update() {
        if (this.TextObject == null) return;

        if (this.StateMachine == null) {
            this.TryToGetStateMachine();
        }

        if (this.StateMachine == null) return;

        if (!(this.StateMachine.CurrentState is RobotFramedState)) {
            this.TextObject.text = this.Label + "–";
        } else {
            RobotFramedState robotFramedState = 
                (RobotFramedState) this.StateMachine.CurrentState;

            this.TextObject.text = this.Label + robotFramedState.CurrentFrame;
        }
    }
}