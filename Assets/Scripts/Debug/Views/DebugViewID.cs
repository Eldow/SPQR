using UnityEngine;

public class DebugViewID : DebugViewer {
    [HideInInspector] public PlayerController PlayerController;

    // to be overriden in child, if necessary
    public override string Label {
        get {
            return "ID: ";
        }
    }

    void Start () {
        this.TryToGetPlayer();
    }

    void Update () {
        if (this.PlayerController == null) {
            this.TryToGetPlayer();
        }

        if (this.PlayerController == null) return;

        this.TextObject.text = this.Label + PlayerController.ID;
    }

    protected virtual void TryToGetPlayer() {
        GameObject player = 
            GameObject.FindGameObjectWithTag(PlayerController.Player);

        if (player == null) return;

        this.PlayerController = player.GetComponent<PlayerController>();
    }
}
