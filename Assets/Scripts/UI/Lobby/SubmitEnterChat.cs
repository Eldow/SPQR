public class SubmitEnterChat : SubmitEnter {
    public FriendChatPannelBehaviour FriendChatPannelBehaviour = null;

    protected override void Callback() {
        if (this.FriendChatPannelBehaviour == null) return;

        this.FriendChatPannelBehaviour.SendMessage();
    }
}
