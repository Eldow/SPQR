using UnityEngine;

public class DebugViewCurrentState : DebugViewer {
    [HideInInspector]
    public StateMachine StateMachine = null;

    // to be overriden in child, if necessary
    public override string Label {
        get {
            return "CS: ";
        }
    }

    void Start () {
        this.tryToGetStateMachine();
    }

	void Update () {
        if (this.TextObject == null) return;

	    if (this.StateMachine == null) {
	        this.tryToGetStateMachine();
	    }

	    if (this.StateMachine == null) return;

	    this.TextObject.text = this.Label + this.StateMachine.CurrentState;
	}

    protected void tryToGetStateMachine() {
        GameObject player =
            GameObject.FindGameObjectWithTag(PlayerController.Player);

        if (player == null) return;

        Automaton automaton = player.GetComponent<Automaton>();

        if (automaton == null) return;

        this.StateMachine = automaton.StateMachine;
    }
}
