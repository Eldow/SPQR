using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class Scoreboard : MonoBehaviour {

    public GameObject TeamEntry;
    public GameObject ScoreEntry;
    public Dictionary<string, GameObject> ActivePlayersEntries;
    public Dictionary<string, int> ActivePlayersVictoryCount;
    public Dictionary<string, int> PlayerTeams;
    public int RoundsToWin;
    public bool RoundGiven = false;

    public GameObject scorePanel;

    private void Start() {

      DontDestroyOnLoad(scorePanel);
      ActivePlayersVictoryCount = new Dictionary<string, int>();

      if (PhotonNetwork.offlineMode) {
        ActivePlayersEntries = InstantiateOfflineScoreboard();
        RoundsToWin = 3;
      }
      else {
        object rounds;
        ActivePlayersEntries = InstantiateOnlineScoreboard();
        PhotonNetwork.room.CustomProperties.TryGetValue ("Mode", out rounds);
        RoundsToWin = (int)rounds * 2 +1;
      }
    }

    private Dictionary<string, GameObject> InstantiateOfflineScoreboard(){
      //Player List
      GameObject list = InstantiateTeamEntry("Players");
      Dictionary<string, GameObject> activePlayers = new Dictionary<string, GameObject>();
      //Player Entry
      activePlayers.Add("Solo", InstantiatePlayerEntry("Solo", list.transform));
      ActivePlayersVictoryCount.Add("Solo", 0);
      //AI Entry
      activePlayers.Add("Computer", InstantiatePlayerEntry("Computer", list.transform));
      ActivePlayersVictoryCount.Add("Computer", 0);

      return activePlayers;
    }

    private Dictionary<string, GameObject> InstantiateOnlineScoreboard(){

      //Fetches the teams custom room properties.
      object teams;
      PhotonNetwork.room.CustomProperties.TryGetValue ("Teams", out teams);
      PlayerTeams = (Dictionary<string, int>)teams;

      //Dictionary that contains the instantiated teams keyed by their number.
      Dictionary<int, GameObject> usedTeams = new Dictionary<int, GameObject>();

      Dictionary<string, GameObject> activePlayers = new Dictionary<string, GameObject>();

      foreach(KeyValuePair<string, int> entry in PlayerTeams) {
        //Instantiate team entry if it doesn't already exist.
        if (!usedTeams.ContainsKey(entry.Value)) {
          usedTeams.Add(entry.Value, InstantiateTeamEntry("Team n°"+entry.Value.ToString()));
        }
        //Instantiate the player entry.
        activePlayers.Add(entry.Key, InstantiatePlayerEntry(entry.Key, usedTeams[entry.Value].transform));
        ActivePlayersVictoryCount.Add(entry.Key, 0);
      }

      return activePlayers;
    }

    private GameObject InstantiatePlayerEntry(string PlayerName, Transform parent){
      GameObject player = Instantiate(ScoreEntry, parent);
      Text playerName = player.transform.GetChild(0).gameObject.GetComponent<Text>();
      Text playerScore = player.transform.GetChild(1).gameObject.GetComponent<Text>();
      player.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
      playerName.text = PlayerName;
      playerScore.text = "";
      return player;
    }

    private GameObject InstantiateTeamEntry(string TeamName){
      GameObject list = Instantiate(TeamEntry, this.transform);
      Text listName = list.transform.GetChild(0).gameObject.GetComponent<Text>();
      listName.text = "   "+TeamName;
      list.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
      return list;
    }

    public void AddVictory(string teamColor, string VictoryType){
      if (!RoundGiven) {
        if (PhotonNetwork.offlineMode) {
            if (teamColor == "White"){
               Text playerScore = ActivePlayersEntries["Solo"].transform.GetChild(1).gameObject.GetComponent<Text>();
               playerScore.text += VictoryType;
               ActivePlayersVictoryCount["Solo"]++;
            }
            else {
              Text playerScore = ActivePlayersEntries["Computer"].transform.GetChild(1).gameObject.GetComponent<Text>();
              playerScore.text += VictoryType;
              ActivePlayersVictoryCount["Computer"]++;
            }
        }
        else {
          foreach(KeyValuePair<string, int> player in PlayerTeams){
             if (player.Value == ColorToInt(teamColor)) {
               Text playerScore = ActivePlayersEntries[player.Key].transform.GetChild(1).gameObject.GetComponent<Text>();
               playerScore.text += VictoryType;
               ActivePlayersVictoryCount[player.Key]++;
             }
          }
        }
        RoundGiven = true;
      }
    }

    public int ColorToInt (string teamColor){
        switch (teamColor){
            case "White":
                return 1;
                break;
            case "Black":
                return 2;
                break;
            case "Blue":
                return 3;
                break;
            case "Red":
                return 4;
                break;
            case "Green":
                return 5;
                break;
            case "Orange":
                return 6;
                break;
            case "Violet":
                return 7;
                break;
            case "Cyan":
                return 8;
                break;
            default:
                return 8;
        }
    }

    public bool CheckForGameVictory(){
       int WinningTeam = CheckTeamScores();
       if  (WinningTeam < 8) {
         return true;
       }
       else return false;
    }

    public int CheckTeamScores(){
       foreach(KeyValuePair<string, int> player in PlayerTeams) {
          if (ActivePlayersVictoryCount[player.Key] >= RoundsToWin ) return player.Value;
       }
       return 8;
    }

}
