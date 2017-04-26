public class SubmitEnterFriend : SubmitEnter {
    public FriendListManager FriendListManager = null;

    protected override void Callback() {
        if (this.FriendListManager == null) return;

        this.FriendListManager.AddFriend();
    }
}
