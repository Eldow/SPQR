using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotHistoryStateMachine : RobotStateMachine {
    [HideInInspector] public Stack StateHistory;
    public int MaxHistorySize = 12;

    void Start() {
        base.Initialize();
        this.StateHistory = new Stack(this.MaxHistorySize);
    }
}
