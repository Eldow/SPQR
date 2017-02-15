using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAutomata : Automata {
    public RobotStateMachine StateMachine;
    public RobotHistoryStateMachine SpecialStateMachine;

	void Start () {
	    this.StateMachine = this.gameObject.AddComponent<RobotStateMachine>();
        this.SpecialStateMachine = 
            this.gameObject.AddComponent<RobotHistoryStateMachine>();
	}

	void Update () {
	}
}
