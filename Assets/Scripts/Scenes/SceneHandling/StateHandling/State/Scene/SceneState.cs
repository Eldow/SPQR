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

    public override void Update(StateMachine stateMachine) {

    }

    public override void Enter(StateMachine stateMachine) {
        if (!(stateMachine is SceneStateMachine)) return;

        ((SceneStateMachine)stateMachine).LoadScene(this.SceneName);
    }

    public override void Exit(StateMachine stateMachine) {
        if (!(stateMachine is SceneStateMachine)) return;

        ((SceneStateMachine)stateMachine).UnloadScene(this.SceneName);
    }
}
