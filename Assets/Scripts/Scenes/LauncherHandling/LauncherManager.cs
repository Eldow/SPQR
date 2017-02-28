using UnityEngine;
using UnityEngine.SceneManagement;

public class LauncherManager : Photon.PunBehaviour
{

    public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;
    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    public byte MaxPlayersPerRoom = 2;

    public string LevelToLoad = "Sandbox";

    private string _gameVersion = "0.09";
    private bool _isConnecting;

    void Awake() {
        PhotonNetwork.logLevel = Loglevel;
        PhotonNetwork.autoJoinLobby = false;
        PhotonNetwork.automaticallySyncScene = true;
    }

    void Start() {
    }

    public virtual void Local() {
        SceneManager.LoadScene(this.LevelToLoad);
    }

    public virtual void Connect() {
        _isConnecting = true;

        if (PhotonNetwork.connected) {
            PhotonNetwork.JoinRandomRoom();

            return;
        }

        PhotonNetwork.ConnectUsingSettings(_gameVersion);
    }

    public override void OnConnectedToMaster() {
        Debug.Log("Launcher: OnConnectedToMaster() was called by PUN");

        if (_isConnecting) {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnDisconnectedFromPhoton()
    {
        Debug.LogWarning("Launcher: OnDisconnectedFromPhoton() was called by PUN");
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        Debug.Log("Launcher:OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 2}, null);");
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = MaxPlayersPerRoom }, null);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        Debug.Log("Player Name: " + PhotonNetwork.playerName.ToString());
        PhotonNetwork.LoadLevel(this.LevelToLoad);
    }

}
