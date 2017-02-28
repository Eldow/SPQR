using UnityEngine.SceneManagement;

public class LauncherManager : Photon.PunBehaviour {
    public const string GameVersion = "0.09";

    public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;
    public byte MaxPlayersPerRoom = 2;
    public string LevelToLoad = "Sandbox";

    private bool _isConnecting;

    void Awake() {
        PhotonNetwork.logLevel = Loglevel;
        PhotonNetwork.autoJoinLobby = false;
        PhotonNetwork.automaticallySyncScene = true;
    }

    void Start() {
        
    }

    public virtual void Local() {
        PhotonNetwork.offlineMode = true;
        PhotonNetwork.CreateRoom("OfflineRoom");
    }

    public virtual void Connect() {
        this._isConnecting = true;

        if (PhotonNetwork.connected) {
            PhotonNetwork.JoinRandomRoom();

            return;
        }

        PhotonNetwork.ConnectUsingSettings(LauncherManager.GameVersion);
    }

    public override void OnConnectedToMaster() {
        if (this._isConnecting) {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnDisconnectedFromPhoton() {}

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg) {
        PhotonNetwork.CreateRoom(null, 
            new RoomOptions() {MaxPlayers = MaxPlayersPerRoom}, 
            null
        );
    }

    public override void OnJoinedRoom() {
        PhotonNetwork.LoadLevel(this.LevelToLoad);
    }
}
