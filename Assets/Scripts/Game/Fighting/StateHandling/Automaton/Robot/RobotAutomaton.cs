/* RobotAutomaton is an object that contains all the FSMs and other useful
 * attributes / methods necessary for it. */
public class RobotAutomaton : Automaton {
    void Awake() {
        if ((this.StateMachine =
                this.gameObject.GetComponent<RobotStateMachine>()) != null) {
            return;
        }

        this.StateMachine = this.gameObject.AddComponent<RobotStateMachine>();
    }
}
