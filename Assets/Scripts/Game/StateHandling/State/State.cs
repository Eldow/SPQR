﻿public class State {
    protected virtual void Initialize() {
    }

    public virtual State HandleInput(StateMachine stateMachine) {
	      return null;
	  }

	  public virtual void Update(StateMachine stateMachine) {

	  }

	  public virtual void Enter(StateMachine stateMachine) {

	  }

	  public virtual void Exit(StateMachine stateMachine) {

	  }
}
