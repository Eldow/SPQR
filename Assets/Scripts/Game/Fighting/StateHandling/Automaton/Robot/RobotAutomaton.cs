using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAutomaton : Automaton {
    public RobotStateMachine StateMachine;

	void Start () {
	    this.StateMachine = this.gameObject.AddComponent<RobotStateMachine>();
	}

	void Update () {
	}
}
