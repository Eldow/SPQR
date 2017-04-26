using UnityEngine;
using UnityEngine.UI;

public class SubmitRoomChatPanel : SubmitEnter {
    public ChatManager ChatManager = null;
    public ScrollRect ScrollRect = null;
    public Scrollbar Scrollbar = null;
    public Canvas Canvas = null;

    public override void Callback() {
        if (this.ChatManager == null || this.ScrollRect == null ||
            this.Scrollbar == null || this.Canvas == null) {
            return;
        }

        this.ChatManager.SendPrivateMessage();

        Canvas.ForceUpdateCanvases();
        this.ScrollRect.verticalNormalizedPosition = 0.0f;
        this.Scrollbar.value = 0.0f;
    }
}

