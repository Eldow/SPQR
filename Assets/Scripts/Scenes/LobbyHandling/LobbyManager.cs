using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : Photon.PunBehaviour
{
    private string _mapLabel = "Sandbox";
    private string _gameMode = "BO3";
    private bool _visibility = true;

    public GameObject MatchmakingPanel;
    public GameObject CustomPanel;

    // Initialize the room list panel
    private void Start()
    {
        RefreshRoomList();
        //PhotonNetwork.CreateRoom(PhotonNetwork.playerName + ":Room");
    }


    // Create a room and switch the list panel to the detail panel
    public void CreateRoom()
    {
        /*
        PhotonNetwork.CreateRoom(null,
            new RoomOptions() { MaxPlayers = _roomSize,  IsVisible = _visibility },
            null
        );*/
    }


    // Apply filters and refresh the room list
    public void RefreshRoomList()
    {
        foreach (RoomInfo game in PhotonNetwork.GetRoomList())
        {
            Debug.Log(game.Name);
            Debug.Log(game.PlayerCount);
            Debug.Log(game.MaxPlayers);
        }
    }


    public void StartGame()
    {
        ChatManager chat = GameObject.Find("ChatManager").GetComponent<ChatManager>();
        ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
        h.Add("Teams", chat.PlayerTeams);
        PhotonNetwork.CreateRoom(PhotonNetwork.playerName + "Room", new RoomOptions() { CustomRoomProperties = h, MaxPlayers = System.Convert.ToByte(chat.PlayerTeams.Count) }, null);
        MatchmakingPanel.transform.FindChild("ButtonsBottom/CreateGameButton").GetComponent<Button>().enabled = false;
        //Debug.Log(_roomSize + _mapLabel + _gameMode + _visibility);
        //PhotonNetwork.LoadLevel(_mapLabel);
        //PhotonNetwork.LoadLevel("Sandbox");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Player joined");
        ChatManager chat = GameObject.Find("ChatManager").GetComponent<ChatManager>();
        chat.SendStartGame(PhotonNetwork.room.Name);
        chat.SayReady();
    }

    public void MatchmakingView()
    {
        CustomPanel.SetActive(false);
        MatchmakingPanel.SetActive(true);
    }

    public void CustomView()
    {
        MatchmakingPanel.SetActive(false);
        CustomPanel.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }
}