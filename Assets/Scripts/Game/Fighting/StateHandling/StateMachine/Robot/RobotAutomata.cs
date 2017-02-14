using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAutomata : MonoBehaviour {
    private RobotStateMachine _stateMachine;
    private RobotHistoryStateMachine _specialStateMachine;

	void Start () {
	    this._stateMachine = this.gameObject.AddComponent<RobotStateMachine>();
        this._specialStateMachine = 
            this.gameObject.AddComponent<RobotHistoryStateMachine>();
	}

	void Update () {
	}
}
