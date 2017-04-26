public class SubmitRoomChatPanel : SubmitEnter {
    public ChatManager ChatManager = null;

    protected override void Callback() {
        if (this.ChatManager == null) return;

        this.ChatManager.SendPrivateMessage();
    }
}

