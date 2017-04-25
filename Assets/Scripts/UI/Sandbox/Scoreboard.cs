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

    public Dictionary<string, int> PlayerTeams;

    public Dictionary<int, int> TeamVictories = new Dictionary<int, int>();
    public Dictionary<int, GameObject> TeamPanels = new Dictionary<int, GameObject>();

    public int RoundsToWin;
    public GameObject scorePanel;
    public Text GamemodeLabel;

    public string VictoryLabel = "X";

    private string _scoreKey = "Scores";

    private void Start()
    {
        PlayerTeams = GameObject.Find("GameManager").GetComponent<NetworkGameManager>().PlayerTeams;
        InstantiateScoreboard();
        if (PhotonNetwork.offlineMode)
        {
            RoundsToWin = 3;
        }
        else
        {
            object rounds;
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

    private void InstantiateScoreboard()
    {
        //Dictionary that contains the instantiated teams keyed by their number.

        Dictionary<string, GameObject> activePlayers = new Dictionary<string, GameObject>();

        foreach (KeyValuePair<string, int> entry in PlayerTeams)
        {
            //Instantiate team entry if it doesn't already exist.
            if (!TeamPanels.ContainsKey(entry.Value))
            {
                TeamPanels.Add(entry.Value, InstantiateTeamEntry(((PlayerColors)entry.Value).ToString()));
                TeamVictories.Add(entry.Value, 0);
            }
        }
    }

    private GameObject InstantiateTeamEntry(string TeamName)
    {
        GameObject list = Instantiate(TeamEntry, this.transform);
        Text listName = list.transform.GetChild(0).gameObject.GetComponent<Text>();
        listName.text = "";
        list.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        Image backgroundImage = list.GetComponent<Image>();
        Color color = Color.clear;

        ColorUtility.TryParseHtmlString(TeamName, out color);

        backgroundImage.color = color;

        listName.color = new Color(1.0f - color.r, 1.0f - color.g, 1.0f - color.b); ;
        return list;
    }

    public void AddVictory(string teamColor)
    {
        if (teamColor == "")
        {
            GameManager.Instance.SetRoundFinished();// Players died at the same moment ? Let's call it a draft
            return;
        }
        PlayerColors color = (PlayerColors)Enum.Parse(typeof(PlayerColors), teamColor, true);
        int colorIndex = (int)color;
        if (TeamVictories.ContainsKey(colorIndex)){
            TeamVictories[colorIndex]++;
        }
        SaveScoreboardToCustomProperties();
    }

    private void UpdatePlayerScoreEntries()
    {
        foreach (KeyValuePair<int, int> team in TeamVictories)
        {
            Text teamScore = TeamPanels[team.Key].transform.GetChild(0).gameObject.GetComponent<Text>();
            teamScore.text = "";
            for (int i = 0; i < team.Value; i++)
            {
                teamScore.text += VictoryLabel;
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
        foreach (KeyValuePair<int, int> team in TeamVictories)
        {
            if (TeamVictories[team.Key] >= Mathf.Ceil(RoundsToWin/2)+1) return team.Value;
        }
        return -1;
    }

    private void SaveScoreboardToCustomProperties()
    {
        ExitGames.Client.Photon.Hashtable scoresHashtable = new ExitGames.Client.Photon.Hashtable();
        scoresHashtable[_scoreKey] = TeamVictories;
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

    private void UpdateVictoryCountFromCustomProperties()
    {
        object scores;
        PhotonNetwork.room.CustomProperties.TryGetValue("Scores", out scores);
        TeamVictories = (Dictionary<int, int>)scores;
        UpdatePlayerScoreEntries();
    }

}
