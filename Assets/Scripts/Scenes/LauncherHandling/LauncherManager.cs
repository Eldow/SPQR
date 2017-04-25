using UnityEngine.SceneManagement;
using UnityEngine;


public class LauncherManager : Photon.PunBehaviour {
    public const string GameVersion = "0.09";
    public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;
    public byte MaxPlayersPerRoom = 2;
    public string LevelToLoad = "Sandbox";

    private bool _isConnecting;
    private static string _chatAppID = "42d0ea52-7bbb-43b0-afd5-eb85c0c1d2bf";

    void Awake() {
        PhotonNetwork.logLevel = Loglevel;
        PhotonNetwork.autoJoinLobby = false;
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.PhotonServerSettings.ChatAppID = _chatAppID;
		PhotonNetwork.offlineMode = false;
    }

    void Start() {
        
    }

    public virtual void Local() {
        PhotonNetwork.Disconnect();
        PhotonNetwork.offlineMode = true;
		PhotonNetwork.CreateRoom("OfflineRoom"+Random.Range(0,100000));
    }

    public virtual void Connect() {
        this._isConnecting = true;

        if (PhotonNetwork.connected) {
            //PhotonNetwork.JoinRandomRoom();
            if (PhotonNetwork.connectedAndReady)
            {
                SceneManager.LoadScene("Lobby");
            }
            return;
        }

        PhotonNetwork.ConnectToRegion(CloudRegionCode.us, LauncherManager.GameVersion);
    }

    public override void OnConnectedToMaster() {
        if (!this._isConnecting) return;

        // BUG: MUST CHECK IF THERE IS A ROOM
        //if (PhotonNetwork.JoinRandomRoom()) return;

        //PhotonNetwork.CreateRoom(null);
        SceneManager.LoadScene("Lobby");
    }

    void OnPhotonRandomJoinFailed() {
        PhotonNetwork.CreateRoom(null);
    }

    public override void OnDisconnectedFromPhoton() {}

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg) {
        /*
        PhotonNetwork.CreateRoom(null, 
            new RoomOptions() {MaxPlayers = MaxPlayersPerRoom}, 
            null
        );*/
    }

    public override void OnJoinedRoom() {
		if(PhotonNetwork.offlineMode)
        	PhotonNetwork.LoadLevel(this.LevelToLoad);
    }

	public void Quit() {
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#endif
			Application.Quit ();
	}
}
