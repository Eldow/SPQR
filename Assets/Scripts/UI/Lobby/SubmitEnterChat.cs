using UnityEngine;
using UnityEngine.UI;

public class SubmitEnterChat : SubmitEnter {
    public FriendChatPannelBehaviour FriendChatPannelBehaviour = null;
    public ScrollRect ScrollRect = null;
    public Scrollbar Scrollbar = null;
    [HideInInspector]
    public Canvas Canvas = null;

    void Start() {
        GameObject LobbyCanvas =
            GameObject.FindGameObjectWithTag("LobbyCanvas");

        if (LobbyCanvas == null) return;

        this.Canvas = LobbyCanvas.GetComponent<Canvas>();
    }

    public override void Callback() {
        if (this.FriendChatPannelBehaviour == null || this.ScrollRect == null ||
            this.Scrollbar == null || this.Canvas == null) {
            return;
        }

        this.FriendChatPannelBehaviour.SendMessage();

        Canvas.ForceUpdateCanvases();
        this.ScrollRect.verticalNormalizedPosition = 0.0f;
        this.Scrollbar.value = 0.0f;
    }
}
