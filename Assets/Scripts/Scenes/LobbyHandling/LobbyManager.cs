using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyManager : Photon.PunBehaviour
{
    private string _mapLabel = "Sandbox";
    private string _gameMode = "BO3";
    private bool _visibility = true;

    public GameObject MatchmakingPanel;
    public GameObject MenuPanel;

    private bool _isMenuActive = false;
    // Initialize the room list panel
    private void Start()
    {
        SoundManager.instance.PlayLobbyMusic();
    }

    void Update()
    {
        if (Input.GetButtonDown("MenuButton"))
        {
            _isMenuActive = !_isMenuActive;
            MenuPanel.SetActive(_isMenuActive);
        }
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
        SoundManager.instance.PlayClick();
        ChatManager chat = GameObject.Find("ChatManager").GetComponent<ChatManager>();
        ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
		chat.preventUniqueTeam ();
        h.Add("Teams", chat.PlayerTeams);
        if(chat.Map == 0)
        {
            chat.Map = Random.Range(1, 4); // 4 because random range is exclusive
        }
        h.Add("Map", chat.Map);
        h.Add("Mode", chat.Mode);
        PhotonNetwork.CreateRoom(PhotonNetwork.playerName + "Room", new RoomOptions() { CustomRoomProperties = h, MaxPlayers = System.Convert.ToByte(chat.PlayerTeams.Count) }, null);
        MatchmakingPanel.transform.FindChild("ButtonsBottom/CreateGameButton").GetComponent<Button>().enabled = false;
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Player joined");
        ChatManager chat = GameObject.Find("ChatManager").GetComponent<ChatManager>();
        chat.SendStartGame(PhotonNetwork.room.Name);
        chat.SayReady();
    }

    public void Launcher() {
        SoundManager.instance.StopMusic();
        SoundManager.instance.PlayClick();
        SceneManager.LoadScene("Launcher");
    }

    public void Exit()
    {

        SoundManager.instance.PlayClick();

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
            Application.Quit();
    }
}