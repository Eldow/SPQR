using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendChatPannelBehaviour : MonoBehaviour {

	public void CloseChat()
    {
        GameObject chat = GameObject.Find("ChatManager");
        chat.GetComponent<ChatManager>().ClosePanel();
    }

    public void SendMessage()
    {
        GameObject chat = GameObject.Find("ChatManager");
        chat.GetComponent<ChatManager>().SendPrivateMessage();
    }
}
