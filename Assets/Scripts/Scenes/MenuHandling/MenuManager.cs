using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
    public GameObject ControlPanel;
    public GameObject ProgressLabel;

    void Start () {
        this.SetMenu();
    }

    void Update () {
        
    }

    public virtual void StartGame(bool isLocal = false) {
        LauncherManager launcherManager =
            this.gameObject.AddComponent<LauncherManager>();

        if (launcherManager == null) return;

        this.SetNetworkingMenu();
        if (!isLocal) {
            launcherManager.Connect();

            return;
        }

        launcherManager.Local();
    }

    public virtual void SetMenu() {
        ProgressLabel.SetActive(false);
        ControlPanel.SetActive(true);
    }

    public virtual void SetNetworkingMenu() {
        ProgressLabel.SetActive(true);
        ControlPanel.SetActive(false);
    }
}
