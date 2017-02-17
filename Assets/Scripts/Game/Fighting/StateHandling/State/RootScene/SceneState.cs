using System;
using UnityEngine;

public class SceneState : State {
    public string SceneName { get; protected set; }

    // to be changed in a child class, if necessary
    public virtual string DefaultSceneName {
        get {
            return "";
        }
    }

    public SceneState(string sceneName = null) {
        if (String.IsNullOrEmpty(sceneName)) {
            sceneName = this.DefaultSceneName;
        }

        this.SceneName = sceneName;
    }

    public virtual void Update(SceneStateMachine stateMachine) {

    }

    protected virtual void 
        Enter(SceneStateMachine stateMachine) {
        if (!stateMachine.LoadScene(this.SceneName)) {
            Debug.LogError(this.SceneName + ": unknown scene to load!");
        }
    }

    public virtual void Exit(SceneStateMachine stateMachine) {
        if (!stateMachine.UnloadScene(this.SceneName)) {
            Debug.LogError(this.SceneName + ": error while unloading!");
        }
    }
}
