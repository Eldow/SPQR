using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FriendListManager : MonoBehaviour {

    public GameObject FriendPrefab;
    private static string _friendListKey = "FriendList";

    private Dictionary<string, GameObject> _friendList = new Dictionary<string, GameObject>();

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
        Debug.Log(PlayerPrefs.GetString(_friendListKey));
        PhotonNetwork.FindFriends(PlayerPrefs.GetString(_friendListKey).Split("*".ToCharArray()));
    }

    public void OnUpdatedFriendList()
    {
        ShowFriendList();
    }

    // Display friends
    public void ShowFriendList()
    {
        Transform results = transform.Find("ResultList/ScrollablePanel");
        foreach (Transform child in results)
        {
            GameObject.Destroy(child.gameObject);
        }
        if (PhotonNetwork.Friends == null) return;
        foreach(FriendInfo friend in PhotonNetwork.Friends)
        {
            if(friend.Name != "")
            {
                GameObject newFriend = Instantiate(FriendPrefab, results);
                GameObject Name = newFriend.transform.Find("Name").gameObject;
                Name.GetComponent<Text>().text = friend.Name;
                newFriend.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                newFriend.SetActive(false);
                _friendList.Add(friend.Name, newFriend);
            }
        }
    }

    // Add a friend to PlayerPrefs
    public void AddFriend()
    {
        string name = transform.Find("InputField/Text").GetComponent<Text>().text;
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
        InitFriendList();
        GameObject.Find("ChatManager").GetComponent<ChatManager>().SubscribeToNewFriend(name);
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
        InitFriendList();
        GameObject.Find("ChatManager").GetComponent<ChatManager>().UnsubscribeFromFriend(name);
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
        _friendList.TryGetValue(name, out friend);
        friend.SetActive(true);
    }

    public void SetFriendOffline(string name)
    {
        GameObject friend;
        _friendList.TryGetValue(name, out friend);
        friend.SetActive(false);
    }
}
