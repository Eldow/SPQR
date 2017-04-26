public class SubmitEnterFriend : SubmitEnter {
    public FriendListManager FriendListManager = null;

    public override void Callback() {
        if (this.FriendListManager == null) return;

        this.FriendListManager.AddFriend();
    }
}
