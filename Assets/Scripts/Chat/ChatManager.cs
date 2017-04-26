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
    private int _botCount = 0;
	private Button startButton;

    public GameObject FriendChatPanel;
    public GameObject ChatEntry;
    public GameObject PlayerEntry;
    public GameObject RoomInvitation;
    public GameObject MatchmakingPanel;
    public GameObject FriendList;
    public GameObject Canvas;
    public int MaxHistoryLength = 20;
    public ChatClient ClientChat;
    public Dictionary<string, GameObject> PlayerList = new Dictionary<string, GameObject>();
    public Dictionary<string, int> PlayerTeams = new Dictionary<string, int>();
    public int Map;
    public int Mode;

    // Use this for initialization
    void Start()
    {
        // Protocol
        ConnectionProtocol connectProtocol = ConnectionProtocol.Udp;
        ClientChat = new ChatClient(this, connectProtocol);

        // Region
        ClientChat.ChatRegion = _chatRegion;

        // Player authentication
        ExitGames.Client.Photon.Chat.AuthenticationValues authValues = new ExitGames.Client.Photon.Chat.AuthenticationValues();
        authValues.UserId = PhotonNetwork.playerName;
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
        Canvas.SetActive(true);
        CreateChatRoom();
        ClientChat.SetOnlineStatus(ChatUserStatus.Online);
		startButton = GameObject.Find ("CreateGameButton").GetComponent<Button> ();

    }

    public void OnDisconnected()
    {
        ClientChat.SetOnlineStatus(ChatUserStatus.Offline);
    }

    public bool IsMaster()
    {
        return _chatRoomName.Contains(PhotonNetwork.playerName);
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
            else if (message.ToString().Contains(":Kicked") && message.ToString().Split(':')[0].Equals(PhotonNetwork.playerName))
            {
                LeaveChatRoom();
            }
            // Notify joined to master
            else if(message.ToString().Equals(sender + ":Joined") && isMaster)
            {
                AddPlayerEntry(sender, false);
                SendPlayerList(channelName);
                SendChangeMap(Map);
                SendChangeMode(Mode);
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
                if(_playerReadyCount == PlayerList.Keys.Count - _botCount)
                {
                    PhotonNetwork.LoadLevel("Sandbox");
                }
            }
            // Notify others team changed
            else if (message.ToString().Contains(":Team"))
            {
                string[] tokens = message.ToString().Split(':');
                if (isMaster)
                    PlayerTeams[tokens[0]] = Int32.Parse(tokens[2]);
                if(sender != PhotonNetwork.playerName)
                    SetPlayerTeam(tokens[0], Int32.Parse(tokens[2]));
            }
            else if (message.ToString().Contains(":Map") && !isMaster)
            {
                MatchmakingPanel.transform.FindChild("Filters").GetComponent<FiltersBehaviour>().SetMap(Int32.Parse(message.ToString().Split(':')[0]));
            }
            else if (message.ToString().Contains(":Mode") && !isMaster)
            {
                MatchmakingPanel.transform.FindChild("Filters").GetComponent<FiltersBehaviour>().SetMode(Int32.Parse(message.ToString().Split(':')[0]));
            }
            // Friend chat
            else if(!message.ToString().Contains(":Friends") && !message.ToString().Contains(":Players") &&
                !message.ToString().Contains(":Ready") && !message.ToString().Contains(":Room") && 
                !message.ToString().Contains(":Map") && !message.ToString().Contains(":Mode")
                && !sender.Equals(PhotonNetwork.playerName))
            {
                GameObject chatEntry = Instantiate(ChatEntry, panel.transform.FindChild("ChatLog/ScrollablePanel").transform);
                chatEntry.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                chatEntry.GetComponent<Text>().text = sender + ": " + message.ToString();
            }
        }

		if (PlayerList.Count > 1)
			startButton.interactable = true;
		else 
			startButton.interactable = false;
			
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
        PlayerTeams.Clear();
        PlayerList.Clear();
        _playerReadyCount = 0;
        _botCount = 0;
        Map = 0;
        Mode = 0;
        MatchmakingPanel.transform.FindChild("Filters").GetComponent<FiltersBehaviour>().SetMap(0);
        MatchmakingPanel.transform.FindChild("Filters").GetComponent<FiltersBehaviour>().SetMode(0);
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
        SoundManager.instance.PlayClick();
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
        SoundManager.instance.PlayClick();
        string friendName, message, channelName;
        GameObject panel;
        GameObject button = EventSystem.current.currentSelectedGameObject;
        message = button.transform.parent.FindChild("InputText/Text").GetComponent<Text>().text;
        if (message == "") return;
        button.transform.parent.FindChild("InputText").GetComponent<InputField>().text = "";
        if (button.transform.parent.FindChild("SenderPanel/Text") != null)
        {
            friendName = button.transform.parent.FindChild("SenderPanel/Text").GetComponent<Text>().text;
            channelName = GetChannelName(new string[] { friendName, PhotonNetwork.playerName });
            panel = ShowPanel(GetPanelName(channelName));
            AppendMessageToPanel(message, panel);
        } else
        {
            channelName = _chatRoomName;
            panel = MatchmakingPanel.transform.FindChild("RoomChatPanel").gameObject;
            AppendMessageToPanel(message, panel);
        }
        ClientChat.PublishMessage(channelName, message);
    }

    private void AppendMessageToPanel(string message, GameObject panel)
    {
        GameObject chatEntry = Instantiate(ChatEntry, panel.transform.FindChild("ChatLog/ScrollablePanel").transform);
        chatEntry.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        chatEntry.GetComponent<Text>().text = PhotonNetwork.playerName + ": " + message.ToString();
    }

    public void SendRoomInvitation()
    {
        if (PlayerList.Count >= 8) return;
        string friendName;
        GameObject button = EventSystem.current.currentSelectedGameObject;
        friendName = button.transform.parent.FindChild("Name").GetComponent<Text>().text;

        string channelName = GetChannelName(new string[] { friendName, PhotonNetwork.playerName });
        if (!_friendChannels.ContainsKey(channelName)) return;
        ShowPanel(friendName);
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
        string sender = button.transform.parent.FindChild("Name").GetComponent<Text>().text;
        ShowPanel(sender);
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
                if (PlayerList.ContainsKey(user))
                {
                    RemovePlayerEntry(user, false);
                }
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
        if (PlayerList.Count >= 8) return;
        if (verbose)
        {
            GameObject panel = MatchmakingPanel.transform.FindChild("RoomChatPanel").gameObject;
            GameObject chatEntry = Instantiate(ChatEntry, panel.transform.FindChild("ChatLog/ScrollablePanel").transform);
            chatEntry.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            chatEntry.GetComponent<Text>().text = playerName + " has joined the room.";
        }
        GameObject newPlayer = Instantiate(PlayerEntry, MatchmakingPanel.transform.FindChild("PlayerListContainer/PlayerList"));
        newPlayer.transform.FindChild("Text").GetComponent<Text>().text = playerName;
        newPlayer.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        PlayerList.Add(playerName, newPlayer);
        PlayerTeams.Add(playerName, 1);

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
        PlayerTeams.Remove(playerName);
    }

    public void UpdatePlayerList(string players)
    {
        foreach(string key in PlayerList.Keys)
        {
            GameObject panel;
            PlayerList.TryGetValue(key, out panel);
            Destroy(panel);
        }
        PlayerList.Clear();
        PlayerTeams.Clear();
        PlayerList = new Dictionary<string, GameObject>();
        foreach (string player in players.Split('*'))
        {
            string[] playerAndColor = player.Split('#');
            Debug.Log(player);
            Debug.Log(Int32.Parse(playerAndColor[1]));
            AddPlayerEntry(playerAndColor[0], false);
            PlayerTeams[playerAndColor[0]] = Int32.Parse(playerAndColor[1]);
            SetPlayerTeam(playerAndColor[0], Int32.Parse(playerAndColor[1]));
        }
    }

    public void SendPlayerList(string channelName)
    {
        string playerList = "";
        foreach (string key in PlayerList.Keys)
        {
            if(playerList == "")
            {
                playerList += (key + "#" + PlayerTeams[key].ToString());
            } else
            {
                playerList += ("*" + key + "#" + PlayerTeams[key].ToString());
            }
        }
        ClientChat.PublishMessage(channelName, playerList + ":Players");
    }

    /* 
        Teams
    */
    public void SendModifyTeam(int team)
    {
        ClientChat.PublishMessage(_chatRoomName, PhotonNetwork.playerName + ":Team:" + team.ToString());
    }

    public void SendModifyBot(string botName, int team)
    {
        ClientChat.PublishMessage(_chatRoomName, botName + ":Team:" + team.ToString());
    }
    /*
        Bots
    */
    public void AddBot()
    {
        SoundManager.instance.PlayClick();
        if (PlayerList.Count >= 8) return;
        GameObject newBot = Instantiate(PlayerEntry, MatchmakingPanel.transform.FindChild("PlayerListContainer/PlayerList"));
        newBot.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        _botCount++;
        string playerName = "Bot" + _botCount;
        newBot.transform.FindChild("Text").GetComponent<Text>().text = playerName;
        PlayerList.Add(playerName, newBot);
        PlayerTeams.Add(playerName, 1);
        SendPlayerList(_chatRoomName);
    }

    /*
        Maps & Mode
    */

    public void SendChangeMap(int map)
    {
        Map = map;
        ClientChat.PublishMessage(_chatRoomName, map.ToString() + ":Map");
    }

    public void  SendChangeMode(int mode)
    {
        Mode = mode;
        ClientChat.PublishMessage(_chatRoomName, mode.ToString() + ":Mode");
    }

    // Modify color and update list
    public void SetPlayerTeam(string playerName, int colorIndex)
    {
        GameObject playerEntry;
        PlayerList.TryGetValue(playerName, out playerEntry);
        playerEntry.transform.FindChild("Image").GetComponent<PlayerColorSwitch>().SetPlayerColor(colorIndex);
    }

    public void KickPlayer()
    {
        GameObject button = EventSystem.current.currentSelectedGameObject;
        string playerKicked = button.transform.parent.FindChild("Text").GetComponent<Text>().text;
        if (playerKicked.Contains("Bot"))
        {
            _botCount--;
            RemovePlayerEntry(playerKicked, false);
            SendPlayerList(_chatRoomName);
        } else
        {
            ClientChat.PublishMessage(_chatRoomName, playerKicked + ":Kicked");
        }
        button.SetActive(false);
    }

    // Safe exit
    void OnApplicationQuit()
    {
        if (ClientChat != null) { ClientChat.Disconnect(); }
    }

    void OnDestroy()
    {
        if (ClientChat != null) { ClientChat.Disconnect(); }
    }

	public int getNumberOfTeams(){
		HashSet<int> set = new HashSet<int> ();

		foreach (KeyValuePair<string,int> pair in PlayerTeams) {
			set.Add (pair.Value);
		}
		return set.Count;
	}

	public void preventUniqueTeam(){
		int current = 0;
		if (getNumberOfTeams () == 1) {
			foreach (string key in PlayerList.Keys) {
				PlayerTeams [key] = current;
				current++;
			}
		}
	}
}
