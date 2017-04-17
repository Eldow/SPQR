using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class NetworkGameManager : Photon.PunBehaviour {
	public static int nbPlayersForThisGame;
	public static bool instantiateAI = false;
	public GameObject AIPrefab;
	public GameObject PlayerPrefab;
	public string[] MapSceneNames;
	protected GameObject PlayerAvatar;
	protected PhotonView PhotonView;
	public Dictionary<string, int> PlayerTeams;
	public int Team;
	public PlayerColors Color;
	public static NetworkGameManager Instance = null;


	void Awake() {
		if (NetworkGameManager.Instance == null) {
			NetworkGameManager.Instance = this;
		} else if (NetworkGameManager.Instance != this) {
			Destroy(gameObject);
		}
	}


	void Start() {

		if (!PhotonNetwork.connected) return;
		// Mode init

		// Map init
		object map;
		Vector3 position = new Vector3(0, -0.6f, 0);
		string newMapName;
		int mapIndex = 0;
		if (!PhotonNetwork.offlineMode)
		{
			PhotonNetwork.room.CustomProperties.TryGetValue("Map", out map);
			mapIndex = (int)map;
		}
		else
		{
			mapIndex = Random.Range(0, MapSceneNames.Length);
		}
		newMapName =  MapSceneNames [mapIndex];
		SceneManager.LoadScene (newMapName, LoadSceneMode.Additive);
		// newMapName.transform.localScale = new Vector3(100, 100, 100);
		// Teams init
		object teams;
		if (!PhotonNetwork.offlineMode) {
			PhotonNetwork.room.CustomProperties.TryGetValue ("Teams", out teams);
			PlayerTeams = (Dictionary<string, int>)teams;
			nbPlayersForThisGame = PlayerTeams.Count;
			Team = PlayerTeams [PhotonNetwork.playerName];
		} else {
			PlayerTeams =  new Dictionary<string, int>();
			nbPlayersForThisGame = 2;
			PlayerTeams.Add(PhotonNetwork.playerName, 1);
			PlayerTeams.Add("Bot1", 2);
		}

		//INSTANTIATE Players & AIs
		if (PlayerTeams != null) {
			DistributePlayers();
		}
	}

	private void DistributePlayers()
	{
		string team;
		string robotPrefabName;
		float radius = 9f;
		float angle = 0;
		float step = (2*Mathf.PI)/PlayerTeams.Count;
		float x, z;

		Vector3 spawnPos;
		foreach (string key in PlayerTeams.Keys)
		{
			x = radius * Mathf.Cos(angle);
			z = radius * Mathf.Sin(angle);
			spawnPos = new Vector3(x, 0, z);
			if (key.Contains("Bot") && (PhotonNetwork.isMasterClient || PhotonNetwork.offlineMode))
			{
				instantiateAI = true;
				team = ((PlayerColors)PlayerTeams[key]).ToString();
				robotPrefabName = team + "Robot";

				GameObject temp = PhotonNetwork.InstantiateSceneObject(
					robotPrefabName,
					spawnPos,
					Quaternion.LookRotation(Vector3.zero - spawnPos), 0,null
				);


				/*PhotonView view = temp.GetComponent<PhotonView> ();
				view.ObservedComponents.Add (ai);
				view.ObservedComponents.Add (aiFocus);*/

				temp.transform.name = key + " " + robotPrefabName;
				temp.GetComponent<PlayerController>().Team = team;
				instantiateAI = false;
			}
			else if(key.Equals(PhotonNetwork.playerName))
			{
				Color = (PlayerColors)PlayerTeams[key];
				team = Color.ToString();
				robotPrefabName = team + "Robot";
				GameObject localPlayer = PhotonNetwork.Instantiate(
					robotPrefabName,
					spawnPos,
					Quaternion.LookRotation(Vector3.zero - spawnPos), 0
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
