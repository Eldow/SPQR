using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExitGames.Client.Photon.Chat;
using ExitGames.Client.Photon;
using System;
using UnityEngine.EventSystems;

public class ChatManager : MonoBehaviour, IChatClientListener {

    private static string _playerNamePrefKey = "PlayerName";
    private static string _chatRegion = "US" ;
    private static string _chatVersion = "1.0";

    private int _playerReadyCount = 0;
    private string _chatRoomName;
    private Dictionary<string, GameObject> _friendChannels = new Dictionary<string, GameObject>();

    public GameObject FriendChatPanel;
    public GameObject ChatEntry;
    public GameObject PlayerEntry;
    public GameObject RoomInvitation;
    public GameObject MatchmakingPanel;
    public int MaxHistoryLength = 20;
    public ChatClient ClientChat;
    public Dictionary<string, GameObject> PlayerList = new Dictionary<string, GameObject>();

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        // Protocol
        ConnectionProtocol connectProtocol = ConnectionProtocol.Udp;
        ClientChat = new ChatClient(this, connectProtocol);

        // Region
        ClientChat.ChatRegion = _chatRegion;

        // Player authentication
        ExitGames.Client.Photon.Chat.AuthenticationValues authValues = new ExitGames.Client.Photon.Chat.AuthenticationValues();
        authValues.UserId = PlayerPrefs.GetString(_playerNamePrefKey);
        authValues.AuthType = ExitGames.Client.Photon.Chat.CustomAuthenticationType.None;

        // Connection
        ClientChat.Connect(PhotonNetwork.PhotonServerSettings.ChatAppID, _chatVersion, authValues);
    }

    // Update is called once per frame
    void Update()
    {
        if (ClientChat != null) { ClientChat.Service(); }
    }

    // Debug log
    public void DebugReturn(DebugLevel level, string message)
    {
        if (level == DebugLevel.ERROR)
        {
            Debug.LogError(message);
        }
        else if (level == DebugLevel.WARNING)
        {
            Debug.LogWarning(message);
        }
        else
        {
            Debug.Log(message);
        }
    }

    // Chat state
    public void OnChatStateChange(ChatState state)
    {
        Debug.Log(state.ToString());
    }

    /*
        Connection
    */
    public void OnConnected()
    {
        SubscribeToAllFriends();
        CreateChatRoom();
        ClientChat.SetOnlineStatus(ChatUserStatus.Online);
    }

    public void OnDisconnected()
    {
        ClientChat.SetOnlineStatus(ChatUserStatus.Offline);
    }

    /*
        Room Messages
    */
    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        GameObject panel;
        if (channelName.Contains(":Room"))
        {
            panel = MatchmakingPanel.transform.FindChild("RoomChatPanel").gameObject;
        } else
        {
            panel = ShowPanel(GetPanelName(channelName));
        }
        bool isMaster = GetPanelName(channelName).Equals(PhotonNetwork.playerName);
        for (int i = 0; i < messages.Length; i++)
        {
            object message = messages[i];
            string sender = senders[i];
            // Room chat
            if (message.ToString().Equals(sender + ":Room"))
            {
                if (sender != PhotonNetwork.playerName)
                {
                    GameObject roomInvitation = Instantiate(RoomInvitation, panel.transform.FindChild("ChatLog/ScrollablePanel").transform);
                    roomInvitation.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    roomInvitation.transform.FindChild("RoomName").GetComponent<Text>().text = message.ToString();
                } else
                {
                    GameObject chatEntry = Instantiate(ChatEntry, panel.transform.FindChild("ChatLog/ScrollablePanel").transform);
                    chatEntry.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    chatEntry.GetComponent<Text>().text = "Invite sent";
                }
            }
            // Ask others to update friendlist
            else if (message.ToString().Contains(":Players") && !isMaster)
            {
                UpdatePlayerList(GetPanelName(message.ToString()));
            }
            // Notify joined to master
            else if(message.ToString().Equals(sender + ":Joined") && isMaster)
            {
                AddPlayerEntry(sender, false);
                SendPlayerList(channelName);

            }
            // Notify left to master
            else if (message.ToString().Equals(sender + ":Left") && isMaster)
            {
                RemovePlayerEntry(sender, false);
                SendPlayerList(channelName);
            }
            // Notify others to start game
            else if (message.ToString().Contains(":StartGame") && !isMaster)
            {
                PhotonNetwork.JoinRoom(GetPanelName(message.ToString()));
            }
            else if (message.ToString().Contains(":Ready") && isMaster)
            {
                _playerReadyCount++;
                if(_playerReadyCount == PlayerList.Keys.Count)
                {
                    PhotonNetwork.LoadLevel("Sandbox");
                }
            }
            // Friend chat
            else if(!message.ToString().Contains(":Friends") && !message.ToString().Contains(":Players") &&
                !message.ToString().Contains(":Ready") && !message.ToString().Contains(":Room")
                && !sender.Equals(PhotonNetwork.playerName))
            {
                GameObject chatEntry = Instantiate(ChatEntry, panel.transform.FindChild("ChatLog/ScrollablePanel").transform);
                chatEntry.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                chatEntry.GetComponent<Text>().text = sender + ": " + message.ToString();
            }
        }
    }

    public void SayReady()
    {
        ClientChat.PublishMessage(_chatRoomName, PhotonNetwork.playerName + ":Ready");
    }

    public void SendStartGame(string roomName)
    {
        if(PhotonNetwork.playerName == GetPanelName(_chatRoomName))
            ClientChat.PublishMessage(_chatRoomName, roomName+":StartGame");
    }

    public void CreateChatRoom()
    {
        _chatRoomName = PhotonNetwork.playerName + ":Room";
        foreach (string key in PlayerList.Keys)
        {
            GameObject panel;
            PlayerList.TryGetValue(key, out panel);
            Destroy(panel);
        }
        PlayerList.Clear();
        _playerReadyCount = 0;
        EnterChatRoom(_chatRoomName);
    }

    public void EnterChatRoom(string chatRoomName)
    {
        if(_chatRoomName != chatRoomName)
            CleanChatRoom();
        _chatRoomName = chatRoomName;
        SubscribeToNewChannel(chatRoomName);
        ClientChat.PublishMessage(chatRoomName, PhotonNetwork.playerName + ":Joined");
        if(!chatRoomName.Contains(PhotonNetwork.playerName))
            MatchmakingPanel.GetComponent<MatchmakingPanelBehaviour>().InFriendRoom();
    }

    public void LeaveChatRoom()
    {
        ClientChat.PublishMessage(_chatRoomName, PhotonNetwork.playerName + ":Left");
        UnsubscribeFromChannel(_chatRoomName);
        CleanChatRoom();
        if (!_chatRoomName.Contains(PhotonNetwork.playerName))
            MatchmakingPanel.GetComponent<MatchmakingPanelBehaviour>().OutOfFriendRoom();
        CreateChatRoom();
    }

    public void CleanChatRoom()
    {
        foreach (Transform child in MatchmakingPanel.transform.FindChild("RoomChatPanel/ChatLog/ScrollablePanel"))
        {
            Destroy(child.gameObject);
        }
    }
    /* 
        Private Messages
    */
    public void OnPrivateMessage(string sender, object message, string channelName)
    {
    }

    public void SendPrivateMessage()
    {
        string friendName, message, channelName;
        GameObject panel;
        GameObject button = EventSystem.current.currentSelectedGameObject;
        message = button.transform.parent.FindChild("InputText/Text").GetComponent<Text>().text;
        button.transform.parent.FindChild("InputText").GetComponent<InputField>().text = "";
        if (button.transform.parent.FindChild("SenderPanel/Text") != null)
        {
            friendName = button.transform.parent.FindChild("SenderPanel/Text").GetComponent<Text>().text;
            channelName = GetChannelName(new string[] { friendName, PhotonNetwork.playerName });
            panel = ShowPanel(GetPanelName(channelName));
        } else
        {
            channelName = _chatRoomName;
            panel = MatchmakingPanel.transform.FindChild("RoomChatPanel").gameObject;
        }
        ClientChat.PublishMessage(channelName, message);


        GameObject chatEntry = Instantiate(ChatEntry, panel.transform.FindChild("ChatLog/ScrollablePanel").transform);
        chatEntry.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        chatEntry.GetComponent<Text>().text = PhotonNetwork.playerName + ": " + message.ToString();
    }

    public void SendRoomInvitation()
    {
        string friendName;
        GameObject button = EventSystem.current.currentSelectedGameObject;
        friendName = button.transform.parent.FindChild("Name").GetComponent<Text>().text;

        string channelName = GetChannelName(new string[] { friendName, PhotonNetwork.playerName });
        ClientChat.PublishMessage(channelName, PhotonNetwork.playerName + ":Room");
    }

    public void ClosePanel()
    {
        GameObject button = EventSystem.current.currentSelectedGameObject;
        button.transform.parent.gameObject.SetActive(false);
    }

    public void ShowPanelFromFriendList()
    {
        GameObject button = EventSystem.current.currentSelectedGameObject;
        ShowPanel(button.transform.parent.FindChild("Name").GetComponent<Text>().text);
    }

    public GameObject ShowPanel(string sender)
    {
        GameObject panel;
        string channelName = GetChannelName(new string[] { sender, PhotonNetwork.playerName });

        // Channel has already been instanciated
        if (_friendChannels.ContainsKey(channelName))
        {
            _friendChannels.TryGetValue(channelName, out panel);
            if (!panel.activeInHierarchy)
            {
                panel.SetActive(true);
            }
        }
        // We need to create a new one
        else
        {
            panel = Instantiate(FriendChatPanel, GameObject.Find("ChatPanels").transform);
            panel.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            panel.transform.FindChild("SenderPanel/Text").GetComponent<Text>().text = sender;
            _friendChannels.Add(channelName, panel);
        }
        return panel;
    }

    /*
        Status 
    */ 
    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        Debug.Log("Status Update" + user + " : " + status);
        FriendListManager friendListManager = GameObject.Find("FriendPanel").GetComponent<FriendListManager>();
        switch (status)
        {
            case 2:
                friendListManager.SetFriendOnline(user);
                SubscribeToNewChannel(GetChannelName(new string[] { user, PhotonNetwork.playerName }));
                break;
            default:
                friendListManager.SetFriendOffline(user);
                UnsubscribeFromChannel(GetChannelName(new string[] { user, PhotonNetwork.playerName }));
                break;
        }
    }

    /*
        Subscriptions
    */
    public void OnSubscribed(string[] channels, bool[] results)
    {
        Debug.Log("Susbcribed to a new channel");
    }

    public void OnUnsubscribed(string[] channels)
    {
        Debug.Log("Unsusbcribed from a channel");
    }

    public void SubscribeToNewChannel(string channelName)
    {
        ClientChat.Subscribe(new string[] { channelName });
    }

    public void UnsubscribeFromChannel(string channelName)
    {
        ClientChat.Unsubscribe(new string[] { channelName });
    }

    private void SubscribeToAllFriends()
    {
        List<string> results = new List<string>();
        List<string> friends = new List<string>();
        if (PhotonNetwork.Friends == null) return;
        foreach(FriendInfo friend in PhotonNetwork.Friends)
        {
            friends.Add(friend.Name);
            results.Add(GetChannelName(new string[] { friend.Name, PhotonNetwork.playerName }));
        }
        ClientChat.AddFriends(friends.ToArray());
        //ClientChat.Subscribe(results.ToArray(), MaxHistoryLength);
    }

    public void SubscribeToNewFriend(string friendName)
    {
        ClientChat.Subscribe(new string[] { friendName, PhotonNetwork.playerName });
        ClientChat.AddFriends(new string[] { friendName });
    }

    public void UnsubscribeFromFriend(string friendName)
    {
        ClientChat.Unsubscribe(new string[] { friendName, PhotonNetwork.playerName });
        ClientChat.RemoveFriends(new string[] { friendName });
    }

    // Alphabetically sort array of strings and concatenate in a string
    private string GetChannelName(string[] array)
    {
        Array.Sort(array, (x, y) => String.Compare(x, y));
        string channelName = "";
        foreach (string name in array)
        {
            if (channelName == "")
                channelName += name;
            else
                channelName += ":" + name;
        }
        return channelName;
    }

    private string GetPanelName(string channelName)
    {
        string firstToken, secondToken;
        firstToken = channelName.Split(':')[0];
        secondToken = channelName.Split(':')[1];
        if(secondToken == "Room" || secondToken == "StartGame")
        {
            return firstToken;
        }
        if(firstToken == PhotonNetwork.playerName)
        {
            return secondToken;
        }
        return firstToken;
    }

    /*
        Player list management
    */
    public void AddPlayerEntry(string playerName, bool verbose)
    {
        if (verbose)
        {
            GameObject panel = MatchmakingPanel.transform.FindChild("RoomChatPanel").gameObject;
            GameObject chatEntry = Instantiate(ChatEntry, panel.transform.FindChild("ChatLog/ScrollablePanel").transform);
            chatEntry.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            chatEntry.GetComponent<Text>().text = playerName + " has joined the room.";
        }
        GameObject newPlayer = Instantiate(PlayerEntry, MatchmakingPanel.transform.FindChild("PlayerList"));
        newPlayer.transform.FindChild("Text").GetComponent<Text>().text = playerName;
        PlayerList.Add(playerName, newPlayer);
    }

    public void RemovePlayerEntry(string playerName, bool verbose)
    {
        if (verbose)
        {
            GameObject panel = MatchmakingPanel.transform.FindChild("RoomChatPanel").gameObject;
            GameObject chatEntry = Instantiate(ChatEntry, panel.transform.FindChild("ChatLog/ScrollablePanel").transform);
            chatEntry.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            chatEntry.GetComponent<Text>().text = playerName + " has left the room.";
        }
        GameObject player;
        PlayerList.TryGetValue(playerName, out player);
        Destroy(player);
        PlayerList.Remove(playerName);
    }

    public void UpdatePlayerList(string players)
    {
        foreach(string key in PlayerList.Keys)
        {
            GameObject panel;
            PlayerList.TryGetValue(key, out panel);
            Destroy(panel);
        }
        PlayerList = new Dictionary<string, GameObject>();
        foreach (string player in players.Split('*'))
        {
            AddPlayerEntry(player, false);
        }
    }

    public void SendPlayerList(string channelName)
    {
        string playerList = "";
        foreach (string key in PlayerList.Keys)
        {
            if(playerList == "")
            {
                playerList += key;
            } else
            {
                playerList += "*" + key;
            }
        }
        ClientChat.PublishMessage(channelName, playerList + ":Players");
    }

    // Safe exit
    void OnApplicationQuit()
    {
        if (ClientChat != null) { ClientChat.Disconnect(); }
    }
}
