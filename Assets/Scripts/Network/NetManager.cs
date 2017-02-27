using UnityEngine;

// Unused - Will be useful to override the Unity's Built-in Network Manager
public class NetManager : MonoBehaviour {
    public string GameVersion = "0.1";
    public GameObject PlayerPrefab;
    public int PlayerMax = 10;

    private const string _roomName = "RoomName";
    private RoomInfo[] _roomList;

    void Start() {
        PhotonNetwork.ConnectUsingSettings(this.GameVersion);
    }

    void OnGUI()
    {
        if (!PhotonNetwork.connected) {
            GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());

            return;
        }

        if (PhotonNetwork.room != null || !PhotonNetwork.insideLobby) return;

        if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server")) {
            this.CreateRoom();
        }

        this.JoinRoom();

    }

    protected virtual void CreateRoom() {
        RoomOptions newRoomOptions = new RoomOptions();

        newRoomOptions.IsVisible = true;
        newRoomOptions.IsOpen = true;
        newRoomOptions.MaxPlayers = (byte) this.PlayerMax;
        newRoomOptions.CustomRoomProperties = 
            new ExitGames.Client.Photon.Hashtable();
        newRoomOptions.CustomRoomProperties.Add("s", "for example level name");

        // makes level name accessible in a room list in the lobby
        newRoomOptions.CustomRoomPropertiesForLobby = 
            new string[] { "s" };

        PhotonNetwork.CreateRoom(
            _roomName + "#" + System.Guid.NewGuid().ToString("N"), 
            newRoomOptions, null
        );
    }

    protected virtual void JoinRoom() {
        if (this._roomList == null) return;

        for (int i = 0; i < _roomList.Length; i++) {
            if (GUI.Button(
                new Rect(100, 250 + (110 * i), 250, 100), 
                "Join " + _roomList[i].Name)) {
                PhotonNetwork.JoinRoom(_roomList[i].Name);
            }
        }
    }

    void OnReceivedRoomListUpdate() {
        _roomList = PhotonNetwork.GetRoomList();
    }

    void OnJoinedRoom() {
        Debug.Log("Connected to Room");
        PhotonNetwork.Instantiate(PlayerPrefab.name, Vector3.up * 5, Quaternion.identity, 0);
    }
}
