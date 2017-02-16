/* RobotAutomaton is an object that contains all the FSMs and other useful
 * attributes / methods necessary for it. */
public class RobotAutomaton : Automaton {
    public RobotStateMachine StateMachine;

	void Start () {
	    this.StateMachine = 
            this.gameObject.AddComponent<RobotStateMachine>();
	}

	void Update () {
	}
}
