/* SceneAutomaton is an object that contains all the FSMs and other useful
 * attributes / methods necessary for it. */

public class SceneAutomaton : Automaton {
    void Start() {
        if ((this.StateMachine =
            this.gameObject.GetComponent<StateMachine>()) != null) {
            return;
        }

        this.StateMachine = this.gameObject.AddComponent<StateMachine>();
    }
}
