using UnityEngine;

public class MainMenuManager : MonoBehaviour {
    private SceneStateMachine _sceneStateMachine = null;

	void Start () {
	    this._sceneStateMachine = 
            this.gameObject.GetComponent<SceneStateMachineBinder>()
            .SceneStateMachine;
	}

    public virtual void LaunchGame() {
        if (!this.CheckSceneStateMachine()) return;

        this._sceneStateMachine.SetState(new SceneStateSandbox());
    }

    protected virtual bool CheckSceneStateMachine() {
        if (this._sceneStateMachine == null) return false;

        return true;
    }
}
