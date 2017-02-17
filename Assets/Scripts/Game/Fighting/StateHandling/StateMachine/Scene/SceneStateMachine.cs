using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStateMachine : StateMachine {
    public string StateToLoadFirst = "";

    // to be changed in a child class, if necessary
    public virtual string DefaultState {
        get {
            return "SandboxSceneState";
        }
    }

    void Start() {
        this.Initialize(this.StateToLoadFirst);
    }

    protected override void Initialize(string startingState = null) {
        Type stateType = this.CheckStartingState(startingState);

        if (stateType == null) return;    

        this.CurrentState = (SceneState)Activator.CreateInstance(stateType);

        this.CurrentState.Enter(this);
    }

    public virtual void LoadScene(string sceneName) {
        StartCoroutine(this.Test(sceneName));
    }

    public virtual IEnumerator Test(string sceneName) {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);

        yield return new WaitForEndOfFrame();
    }

    public virtual bool UnloadScene(string sceneName) {
        AsyncOperation operation = SceneManager.UnloadSceneAsync(sceneName);

        return operation.isDone; // to change
    }
}
