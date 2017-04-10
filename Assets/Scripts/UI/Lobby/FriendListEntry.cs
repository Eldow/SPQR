using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendListEntry : MonoBehaviour {

	public void Chat()
    {
        GameObject.Find("FriendPanel").GetComponent<FriendListManager>().OpenChatPanel();
    }
    public void Delete()
    {
        GameObject.Find("FriendPanel").GetComponent<FriendListManager>().RemoveFriend();
    }
    public void Invite()
    {
        GameObject.Find("FriendPanel").GetComponent<FriendListManager>().InviteToRoom();
    }
}
