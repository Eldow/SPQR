/* RobotAutomaton is an object that contains all the FSMs and other useful
 * attributes / methods necessary for it. */
public class RobotAutomaton : Automaton {
    void Start() {
        if ((this.StateMachine =
                this.gameObject.GetComponent<StateMachine>()) != null) {
            return;
        }

        this.StateMachine = this.gameObject.AddComponent<RobotStateMachine>();
    }
}
