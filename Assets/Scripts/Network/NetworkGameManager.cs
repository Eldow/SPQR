using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class NetworkGameManager : Photon.PunBehaviour {

	public GameObject AIPrefab;
    public GameObject PlayerPrefab;
    protected GameObject PlayerAvatar;
    protected PhotonView PhotonView;
    public Dictionary<string, int> PlayerTeams;
    public int Team;
    public PlayerColors Color;

    void Start() {
		
        if (!PhotonNetwork.connected) return;
        object teams;
		string robotPrefabName;
		if (!PhotonNetwork.offlineMode) {
			PhotonNetwork.room.CustomProperties.TryGetValue ("Teams", out teams);
			PlayerTeams = (Dictionary<string, int>)teams;
			Team = PlayerTeams [PhotonNetwork.playerName];
			Color = (PlayerColors)Team;
			robotPrefabName = Color.ToString () + "Robot";
		} else {
			robotPrefabName = "Robot";
		}

        Debug.Log(robotPrefabName);
        GameObject localPlayer = PhotonNetwork.Instantiate(
            robotPrefabName, 
            Vector3.left * (PhotonNetwork.room.PlayerCount * 2), 
            Quaternion.identity, 0
        );

        GameManager.Instance.LocalPlayer 
            = localPlayer.GetComponent<PlayerController>();
    }

    public override void OnLeftRoom() {
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
    }

    void LoadArena() {
        if (!PhotonNetwork.isMasterClient) return;

        PhotonNetwork.LoadLevel("Sandbox");
    }
}

public enum PlayerColors
{
    Gray, White, Black, Blue, Red, Green, Orange, Violet, Cyan
}
