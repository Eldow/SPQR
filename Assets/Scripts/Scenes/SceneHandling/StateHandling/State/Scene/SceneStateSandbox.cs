using System;
using UnityEngine;

public class SceneStateSandbox : SceneState {
    // to be changed in a child class, if necessary
    public override string DefaultSceneName {
        get {
            return "Sandbox";
        }
    }

    public override void Update(StateMachine stateMachine) {

    }
}
