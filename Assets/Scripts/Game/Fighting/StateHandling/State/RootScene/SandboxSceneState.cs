using System;
using UnityEngine;

public class SandboxSceneState : SceneState {
    // to be changed in a child class, if necessary
    public virtual string DefaultSceneName {
        get {
            return "Sandbox";
        }
    }

    public override void Update(SceneStateMachine stateMachine) {

    }

    protected override void Enter(SceneStateMachine stateMachine) {
        if (!stateMachine.LoadScene(this.SceneName)) {
            Debug.LogError(this.SceneName + ": unknown scene to load!");
        }
    }

    public override void Exit(SceneStateMachine stateMachine) {
        if (!stateMachine.UnloadScene(this.SceneName)) {
            Debug.LogError(this.SceneName + ": error while unloading!");
        }
    }
}
