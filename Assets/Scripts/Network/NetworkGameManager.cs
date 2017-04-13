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
	public static NetworkGameManager Instance = null;


	void Awake() {
		if (NetworkGameManager.Instance == null) {
			GameManager.Instance = this;
		} else if (GameManager.Instance != this) {
			Destroy(gameObject);
		}
	}


    void Start() {
		
        if (!PhotonNetwork.connected) return;
        object teams;
		string robotPrefabName;
		if (!PhotonNetwork.offlineMode) {
			PhotonNetwork.room.CustomProperties.TryGetValue ("Teams", out teams);
			PlayerTeams = (Dictionary<string, int>)teams;
			nbPlayersForThisGame = PlayerTeams.Count;
			Team = PlayerTeams [PhotonNetwork.playerName];
		} else {
			robotPrefabName = PlayerColors.White.ToString () + "Robot";
			PlayerTeams =  new Dictionary<string, int>();
			nbPlayersForThisGame = 2;
			PlayerTeams.Add("Bot1", 2);
		}



		//INSTANTIATE AIs
		if (PhotonNetwork.isMasterClient || PhotonNetwork.offlineMode) {
			instantiateAI = true;
			if (PlayerTeams != null) {
                DistributePlayers();
			}
			instantiateAI = false;
		}
    }

    private void DistributePlayers()
    {
        string team;
        string robotPrefabName;
        float radius = 500f;
        float angle = 0;
        float step = (2*Mathf.PI)/PlayerTeams.Count;
        float x, z;
        Vector3 spawnPos;
        foreach (string key in PlayerTeams.Keys)
        {
            x = radius * Mathf.Cos(angle);
            z = radius * Mathf.Sin(angle);
            spawnPos = new Vector3(x, 0, z);
            if (key.Contains("Bot"))
            {
                team = ((PlayerColors)PlayerTeams[key]).ToString();
                robotPrefabName = team + "Robot";

                GameObject temp = PhotonNetwork.Instantiate(
                                     robotPrefabName,
                                     spawnPos,
                                     Quaternion.identity, 0
                                 );
                temp.AddComponent<AI>();
                temp.AddComponent<AIFocus>();
                temp.transform.name = key + " " + robotPrefabName;
                temp.GetComponent<PlayerController>().Team = team;
            }
            else if(key.Equals(PhotonNetwork.playerName))
            {
                Color = (PlayerColors)Team;
                robotPrefabName = Color.ToString() + "Robot";
                GameObject localPlayer = PhotonNetwork.Instantiate(
                    robotPrefabName,
                    spawnPos,
                    Quaternion.identity, 0
                );
                localPlayer.GetComponent<PlayerController>().Team = Color.ToString();

                GameManager.Instance.LocalPlayer
                = localPlayer.GetComponent<PlayerController>();
            }
            angle += step;
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
