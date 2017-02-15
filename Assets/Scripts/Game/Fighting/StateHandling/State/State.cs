using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State {
	void Start () {
		
	}
	
	void Update () {
		
	}

    public virtual State HandleInput(
        StateMachine stateMachine, XboxInput xboxInput) {
        return null;
    }

    public virtual void Update(StateMachine stateMachine) {
        
    }

    public virtual void Enter(StateMachine stateMachine) {
        
    }

    public virtual void Exit(StateMachine stateMachine) {

    }
}
