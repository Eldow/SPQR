using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStateMachine : StateMachine {
    public string StateToLoadFirst = "";
    protected SceneState SceneState = null;

    // to be changed in a child class, if necessary
    public virtual string DefaultState {
        get {
            return "State";
        }
    }

    void Start() {
        this.Initialize(this.StateToLoadFirst);
    }

    void FixedUpdate() {
        this.SceneState.Update(this);
    }

    protected override void Initialize(string startingState = null) {
        Type stateType = this.CheckStartingState(startingState);

        if (stateType == null) return;

        this.SceneState = 
            (SceneState) Activator.CreateInstance(stateType);
        this.SceneState.Enter(this);
    }

    public virtual bool LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);

        return SceneManager.GetActiveScene().name == sceneName;
    }

    public virtual bool UnloadScene(string sceneName) {
        AsyncOperation operation = SceneManager.UnloadSceneAsync(sceneName);

        return operation.isDone; // to change
    }

    protected virtual void SwitchState(SceneState sceneState) {
        if (sceneState == null) return;

        this.SceneState.Exit(this);
        this.SceneState = sceneState;
        this.SceneState.Enter(this);
    }

    public virtual void SetState(SceneState sceneState) {
        this.SwitchState(sceneState);
    }
}
