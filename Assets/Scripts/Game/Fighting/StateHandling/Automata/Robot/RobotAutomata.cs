using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAutomata : Automata {
    public RobotStateMachine StateMachine;

	void Start () {
	    this.StateMachine = this.gameObject.AddComponent<RobotStateMachine>();
	}

	void Update () {
	}
}
