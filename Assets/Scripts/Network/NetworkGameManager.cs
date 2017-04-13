using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class NetworkGameManager : Photon.PunBehaviour {
	public static int nbPlayersForThisGame;
	public static bool instantiateAI = false;
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
			nbPlayersForThisGame = PlayerTeams.Count;
			Team = PlayerTeams [PhotonNetwork.playerName];
			Color = (PlayerColors)Team;
			robotPrefabName = Color.ToString () + "Robot";
		} else {
			robotPrefabName = PlayerColors.White.ToString () + "Robot";
			PlayerTeams =  new Dictionary<string, int>();
			nbPlayersForThisGame = 2;
			PlayerTeams.Add("Bot1", 2);
		}

        Debug.Log(robotPrefabName);
        GameObject localPlayer = PhotonNetwork.Instantiate(
            robotPrefabName, 
            Vector3.left * (PhotonNetwork.room.PlayerCount * 2), 
            Quaternion.identity, 0
        );
		localPlayer.GetComponent<PlayerController> ().Team = Color.ToString ();

		GameManager.Instance.LocalPlayer 
		= localPlayer.GetComponent<PlayerController>();

		//INSTANTIATE AIs
		if (PhotonNetwork.isMasterClient || PhotonNetwork.offlineMode) {
			instantiateAI = true;
			if (PlayerTeams != null) {
				foreach (string key in PlayerTeams.Keys) {
					if (key.Contains ("Bot")) {
						string team = ((PlayerColors)PlayerTeams [key]).ToString ();
						robotPrefabName = team + "Robot";

						GameObject temp = PhotonNetwork.Instantiate (
							                 robotPrefabName, 
							                 Vector3.left * (PhotonNetwork.room.PlayerCount * 2), 
							                 Quaternion.identity, 0
						                 );
						temp.AddComponent<AI> ();
						temp.AddComponent<AIFocus> ();
						temp.transform.name = key + " " + robotPrefabName;
						temp.GetComponent<PlayerController> ().Team = team;
					}
				}
			}
			instantiateAI = false;
		}
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
