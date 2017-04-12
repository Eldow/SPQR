using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FriendListManager : MonoBehaviour {

    public GameObject FriendPrefab;
    private static string _friendListKey = "FriendList";

    public Color OnlineColor = Color.cyan;
    public Color OfflineColor = Color.gray;

    public Text NewFriend = null;

    public Dictionary<string, GameObject> FriendList = new Dictionary<string, GameObject>();

    // Use this for initialization
    void Start () {
        InitFriendList();
	}
	
	// Update is called once per frame
	void Update () {
	}

    private void InitFriendList()
    {
        if (PhotonNetwork.connected)
        {
            LoadFriendList();
        }
    }

    // Stores the friendlist in PhotonNetwork.Friends
    public void LoadFriendList()
    {

        string[] storedFriends = PlayerPrefs.GetString(_friendListKey).Split("*".ToCharArray());
        foreach (string friend in storedFriends)
        {
            InstantiateOfflineFriend(friend);
        }
    }

    public void InstantiateOfflineFriend(string friend)
    {
        Transform results = transform.Find("ResultList/ScrollablePanel");
        if (friend != "" && !FriendList.ContainsKey(friend))
        {
            GameObject newFriend = Instantiate(FriendPrefab, results);
            GameObject Name = newFriend.transform.Find("Name").gameObject;
            Name.GetComponent<Text>().text = friend;
            newFriend.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            newFriend.GetComponent<Image>().color = OfflineColor;
            FriendList.Add(friend, newFriend);
        }
    }

    public void DestroyFriend(string friend)
    {
        GameObject friendObject;
        FriendList.TryGetValue(friend, out friendObject);
        Destroy(friendObject);
        FriendList.Remove(friend);
    }
    // Add a friend to PlayerPrefs
    public void AddFriend()
    {
        string name = this.NewFriend.text;
        AddFriendByName(name);
        
    }

    public void RemoveFriend()
    {
        GameObject button = EventSystem.current.currentSelectedGameObject;
        string name = button.transform.parent.transform.Find("Name").GetComponent<Text>().text;
        RemoveFriendByName(name);
    }

    public void AddFriendByName(string name)
    {
        string friends = PlayerPrefs.GetString(_friendListKey);

        // Avoid duplicates or owner name
        if (name == PhotonNetwork.playerName) return;
        foreach (string playerName in friends.Split("*".ToCharArray()))
        {
            if (playerName == name)
            {
                return;
            }
        }

        if (PlayerPrefs.GetString(_friendListKey) != "")
            friends += "*" + name;
        else
            friends += name;
        PlayerPrefs.SetString(_friendListKey, friends);
        GameObject.Find("ChatManager").GetComponent<ChatManager>().SubscribeToNewFriend(name);
        InstantiateOfflineFriend(name);
    }

    public void RemoveFriendByName(string name)
    {
        string friends = "";
        foreach (string playerName in PlayerPrefs.GetString(_friendListKey).Split("*".ToCharArray()))
        {
            if (playerName != name)
            {
                if (friends == "")
                {
                    friends += playerName;
                }
                else
                {
                    friends += "*" + playerName;
                }
            }
        }
        PlayerPrefs.SetString(_friendListKey, friends);
        GameObject.Find("ChatManager").GetComponent<ChatManager>().UnsubscribeFromFriend(name);
        DestroyFriend(name);
    }

    public void OpenChatPanel()
    {
        GameObject chat = GameObject.Find("ChatManager");
        chat.GetComponent<ChatManager>().ShowPanelFromFriendList();
    }

    public void InviteToRoom()
    {
        GameObject chat = GameObject.Find("ChatManager");
        chat.GetComponent<ChatManager>().SendRoomInvitation();
    }

    public void AcceptInvitation()
    {
        GameObject button = EventSystem.current.currentSelectedGameObject;
        string roomName = button.transform.parent.FindChild("RoomName").GetComponent<Text>().text;
        GameObject chat = GameObject.Find("ChatManager");
        chat.GetComponent<ChatManager>().EnterChatRoom(roomName);
        Destroy(button.transform.parent.gameObject);
    }

    public void DeclineInvitation()
    {
        GameObject button = EventSystem.current.currentSelectedGameObject;
        Destroy(button.transform.parent.gameObject);
    }

    public void SetFriendOnline(string name)
    {
        GameObject friend;
        FriendList.TryGetValue(name, out friend);
        friend.GetComponent<Image>().color = OnlineColor;
    }

    public void SetFriendOffline(string name)
    {
        GameObject friend;
        FriendList.TryGetValue(name, out friend);
        friend.GetComponent<Image>().color = OfflineColor;
    }
}
