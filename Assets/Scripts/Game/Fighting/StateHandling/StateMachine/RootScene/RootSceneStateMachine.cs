public class RootSceneStateMachine : StateMachine {
    // to be changed in a child class, if necessary
    public virtual string DefaultState {
        get {
            return "State";
        }
    }
}
