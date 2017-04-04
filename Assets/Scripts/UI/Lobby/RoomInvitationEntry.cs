using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInvitationEntry : MonoBehaviour {
    public void Accept()
    {
        GameObject.Find("FriendPanel").GetComponent<FriendListManager>().AcceptInvitation();
    }
    public void Decline()
    {
        GameObject.Find("FriendPanel").GetComponent<FriendListManager>().DeclineInvitation();
    }
}
