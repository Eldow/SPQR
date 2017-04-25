using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;

public class Scoreboard : Photon.MonoBehaviour
{

    public GameObject TeamEntry;
    public GameObject ScoreEntry;
    public Dictionary<string, GameObject> ActivePlayersEntries;
    public Dictionary<string, int> ActivePlayersVictoryCount;
    public Dictionary<string, int> PlayerTeams;
    public int RoundsToWin;
    public GameObject scorePanel;
    public Text GamemodeLabel;

    public string VictoryLabel = "X";

    private string _scoreKey = "Scores";

    private void Start()
    {
        PlayerTeams = GameObject.Find("GameManager").GetComponent<NetworkGameManager>().PlayerTeams;
        ActivePlayersVictoryCount = new Dictionary<string, int>();

        if (PhotonNetwork.offlineMode)
        {
            ActivePlayersEntries = InstantiateScoreboard();
            RoundsToWin = 3;
        }
        else
        {
            object rounds;
            ActivePlayersEntries = InstantiateScoreboard();
            PhotonNetwork.room.CustomProperties.TryGetValue("Mode", out rounds);
            // Any mode selected
            if ((int)rounds == 0)
            {
                RoundsToWin = 3;
            }
            // BO mode selected
            else
            {
                RoundsToWin = (int)rounds * 2 + 1;
            }
        }
        GamemodeLabel.text = "BO " + RoundsToWin.ToString();
        scorePanel.SetActive(false);
        LoadScoreboardFromCustomProperties();
    }

    private Dictionary<string, GameObject> InstantiateScoreboard()
    {

        //Dictionary that contains the instantiated teams keyed by their number.
        Dictionary<int, GameObject> usedTeams = new Dictionary<int, GameObject>();

        Dictionary<string, GameObject> activePlayers = new Dictionary<string, GameObject>();

        foreach (KeyValuePair<string, int> entry in PlayerTeams)
        {
            //Instantiate team entry if it doesn't already exist.
            if (!usedTeams.ContainsKey(entry.Value))
            {
                usedTeams.Add(entry.Value, InstantiateTeamEntry("Team " + (PlayerColors)entry.Value));
            }
            //Instantiate the player entry.
            activePlayers.Add(entry.Key, InstantiatePlayerEntry(entry.Key, usedTeams[entry.Value].transform));
            ActivePlayersVictoryCount.Add(entry.Key, 0);
        }

        return activePlayers;
    }

    private GameObject InstantiatePlayerEntry(string PlayerName, Transform parent)
    {
        GameObject player = Instantiate(ScoreEntry, parent);
        Text playerName = player.transform.GetChild(0).gameObject.GetComponent<Text>();
        Text playerScore = player.transform.GetChild(1).gameObject.GetComponent<Text>();
        player.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        playerName.text = PlayerName;
        playerScore.text = "";
        return player;
    }

    private GameObject InstantiateTeamEntry(string TeamName)
    {
        GameObject list = Instantiate(TeamEntry, this.transform);
        Text listName = list.transform.GetChild(0).gameObject.GetComponent<Text>();
        listName.text = "   " + TeamName;
        list.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        Color color;

        try { 
            color = (Color)typeof(Color).GetProperty(TeamName.Replace(@"Team ", "").ToLowerInvariant()).GetValue(null, null);
        } catch (NullReferenceException exception) { 
            color = Color.white;
        }

        listName.color = color;
        return list;
    }

    public void AddVictory(string teamColor)
    {
        foreach (KeyValuePair<string, int> player in PlayerTeams)
        {
            if (player.Value == (int)(PlayerColors)Enum.Parse(typeof(PlayerColors), teamColor, true))
            {
                ActivePlayersVictoryCount[player.Key]++;
            }
        }
        SaveScoreboardToCustomProperties();
    }

    private void UpdatePlayerScoreEntries()
    {
        foreach (KeyValuePair<string, int> player in ActivePlayersVictoryCount)
        {
            Text playerScore = ActivePlayersEntries[player.Key].transform.GetChild(1).gameObject.GetComponent<Text>();
            playerScore.text = "";
            for (int i = 0; i < player.Value; i++)
            {
                playerScore.text += VictoryLabel;
            }
        }
    }

    public bool CheckForGameVictory()
    {
        int WinningTeam = CheckTeamScores();
        if (WinningTeam != -1)
        {
            return true;
        }
        else return false;
    }

    public int CheckTeamScores()
    {
        foreach (KeyValuePair<string, int> player in PlayerTeams)
        {
            if (ActivePlayersVictoryCount[player.Key] >= Mathf.Ceil(RoundsToWin/2)+1) return player.Value;
        }
        return -1;
    }

    private void SaveScoreboardToCustomProperties()
    {
        ExitGames.Client.Photon.Hashtable scoresHashtable = new ExitGames.Client.Photon.Hashtable();
        scoresHashtable[_scoreKey] = ActivePlayersVictoryCount;
        PhotonNetwork.room.SetCustomProperties(scoresHashtable);
    }

    private void LoadScoreboardFromCustomProperties()
    {
        if (PhotonNetwork.room.CustomProperties.ContainsKey(_scoreKey))
        {
            UpdateVictoryCountFromCustomProperties();
        }
    }

    public void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey(_scoreKey))
        {
            UpdateVictoryCountFromCustomProperties();
        }
    }

    private void UpdateVictoryCountFromCustomProperties()
    {
        object scores;
        PhotonNetwork.room.CustomProperties.TryGetValue("Scores", out scores);
        ActivePlayersVictoryCount = (Dictionary<string, int>)scores;
        UpdatePlayerScoreEntries();
        if (CheckForGameVictory())
        {
            GameManager.Instance.SetGameFinished();
        }
        else
        {
            GameManager.Instance.SetRoundFinished();
        }
    }

}
