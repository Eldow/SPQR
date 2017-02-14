using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotHistoryStateMachine : RobotStateMachine {
    [HideInInspector] public Stack StateHistory;
    public int MaxHistorySize = 12;

    public override String DefaultState {
        get {
            return "RobotNoSpecialState";
        }
    }

    void Start() {
        base.Initialize();
        this.StateHistory = new Stack(this.MaxHistorySize);
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
