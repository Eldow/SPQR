using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

// Unused - Will be useful to override the Unity's Built-in Network Manager
public class NetManager : MonoBehaviour
{
    private const string roomName = "RoomName";
    private RoomInfo[] roomsList;
    public GameObject playerPrefab;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("0.1");
    }

    void OnGUI()
    {
        if (!PhotonNetwork.connected)
        {
            GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        }
        else if (PhotonNetwork.room == null && PhotonNetwork.insideLobby)
        {
            // Create Room
            if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
            {
                RoomOptions newRoomOptions = new RoomOptions();
                newRoomOptions.IsVisible = true;
                newRoomOptions.IsOpen = true;
                newRoomOptions.MaxPlayers = 10;
                newRoomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
                newRoomOptions.CustomRoomProperties.Add("s", "for example level name");
                newRoomOptions.CustomRoomPropertiesForLobby = new string[] { "s" }; // makes level name accessible in a room list in the lobby
                PhotonNetwork.CreateRoom(roomName + System.Guid.NewGuid() , newRoomOptions, null);
            }


            // Join Room
            if (roomsList != null)
            {
                for (int i = 0; i < roomsList.Length; i++)
                {
                    if (GUI.Button(new Rect(100, 250 + (110 * i), 250, 100), "Join " + roomsList[i].Name))
                        PhotonNetwork.JoinRoom(roomsList[i].Name);
                }
            }
        }
    }

    void OnReceivedRoomListUpdate()
    {
        roomsList = PhotonNetwork.GetRoomList();
    }

    void OnJoinedRoom()
    {
        Debug.Log("Connected to Room");
        PhotonNetwork.Instantiate(playerPrefab.name, Vector3.up * 5, Quaternion.identity, 0);
    }
}
