using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendListEntry : MonoBehaviour {

	public void Chat()
    {
        SoundManager.instance.PlayClick();
        GameObject.Find("FriendPanel").GetComponent<FriendListManager>().OpenChatPanel();
    }
    public void Delete()
    {
        SoundManager.instance.PlayClick();
        GameObject.Find("FriendPanel").GetComponent<FriendListManager>().RemoveFriend();
    }
    public void Invite()
    {
        SoundManager.instance.PlayClick();
        GameObject.Find("FriendPanel").GetComponent<FriendListManager>().InviteToRoom();
    }
}
