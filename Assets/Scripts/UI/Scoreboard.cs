using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class Scoreboard : MonoBehaviour {

    public GameObject TeamEntry;
    public GameObject ScoreEntry;

    private void Start() {

      if (PhotonNetwork.offlineMode) InstantiateOfflineScoreboard();
      else InstantiateOnlineScoreboard();

    }

    private void InstantiateOfflineScoreboard(){
      //Player List
      GameObject list = InstantiateTeamEntry("Players");
      //Player Entry
      InstantiatePlayerEntry("Solo", list.transform);
      //AI Entry
      InstantiatePlayerEntry("Computer", list.transform);
    }

    private void InstantiateOnlineScoreboard(){

      //Fetches the teams custom room properties.
      object teams;
      PhotonNetwork.room.CustomProperties.TryGetValue ("Teams", out teams);
      Dictionary<string, int> PlayerTeams = (Dictionary<string, int>)teams;

      //Dictionary that contains the instantiated teams keyed by their number.
      Dictionary<int, GameObject> usedTeams = new Dictionary<int, GameObject>();

      foreach(KeyValuePair<string, int> entry in PlayerTeams) {
        //Instantiate team entry if it doesn't already exist.
        if (!usedTeams.ContainsKey(entry.Value)) {
          usedTeams.Add(entry.Value, InstantiateTeamEntry("Team n°"+entry.Value.ToString()));
        }
        //Instantiate the player entry.
        InstantiatePlayerEntry(entry.Key, usedTeams[entry.Value].transform);
      }
    }

    private void InstantiatePlayerEntry(string PlayerName, Transform parent){
      GameObject opponent = Instantiate(ScoreEntry, parent);
      Text opponentName = opponent.transform.GetChild(0).gameObject.GetComponent<Text>();
      Text opponentScore = opponent.transform.GetChild(1).gameObject.GetComponent<Text>();
      opponent.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
      opponentName.text = PlayerName;
      opponentScore.text = "---";
    }

    private GameObject InstantiateTeamEntry(string TeamName){
      GameObject list = Instantiate(TeamEntry, this.transform);
      Text listName = list.transform.GetChild(0).gameObject.GetComponent<Text>();
      listName.text = "   "+TeamName;
      list.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
      return list;
    }

}
