using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStateMachine : StateMachine {
    public string StateToLoadFirst = "";

    // to be changed in a child class, if necessary
    public override string DefaultState {
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
        this.StartCoroutine(this.LoadSceneRoutine(sceneName));
    }

    public virtual IEnumerator LoadSceneRoutine(string sceneName) {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);

        yield return new WaitForEndOfFrame();
    }

    public virtual void UnloadScene(string sceneName) {
        SceneManager.UnloadSceneAsync(sceneName);
    }

    protected override void SwitchState(State state) {
        if (!(state is SceneState)) return;

        base.SwitchState(state);
    }
}
