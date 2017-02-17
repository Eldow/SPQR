using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStateMachineBinder : MonoBehaviour {
    [HideInInspector] public SceneStateMachine SceneStateMachine = null;

    void Start() {
        Scene scene = SceneManager.GetSceneByName("RootScene");

        if (!scene.IsValid()) return;

        StateMachine stateMachine = scene.GetRootGameObjects()[0]
            .GetComponent<SceneAutomaton>().StateMachine;

        if (!(stateMachine is SceneStateMachine)) return;

        this.SceneStateMachine = (SceneStateMachine) stateMachine;
    }
}