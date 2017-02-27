using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
    public string LocalLevelToLoad = "LocalGame";
    public GameObject ControlPanel;
    public GameObject ProgressLabel;

    void Start () {
        this.SetMenu();
    }

    void Update () {
        
    }

    public virtual void StartGame(bool isLocal = false) {
        if (!isLocal) {
            LauncherManager launcherManager = 
                this.gameObject.AddComponent<LauncherManager>();

            if (launcherManager == null) return;

            this.SetNetworkingMenu();

            launcherManager.Connect();

            return;
        }

        SceneManager.LoadScene(this.LocalLevelToLoad);
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
