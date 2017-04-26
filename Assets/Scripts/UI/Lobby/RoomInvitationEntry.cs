using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInvitationEntry : MonoBehaviour {
    public void Accept()
    {
        SoundManager.instance.PlayClick();
        GameObject.Find("FriendPanel").GetComponent<FriendListManager>().AcceptInvitation();
    }
    public void Decline()
    {
        SoundManager.instance.PlayClick();
        GameObject.Find("FriendPanel").GetComponent<FriendListManager>().DeclineInvitation();
    }
}
