using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchmakingPanelBehaviour : MonoBehaviour {

    public GameObject PlayButton;
    public GameObject LeaveButton;
    public GameObject AddBotButton;
    
    void Start()
    {
        OutOfFriendRoom();
    }

    public void OutOfFriendRoom()
    {
        PlayButton.SetActive(true);
        LeaveButton.SetActive(false);
        AddBotButton.SetActive(true);
    }

    public void InFriendRoom()
    {
        PlayButton.SetActive(false);
        LeaveButton.SetActive(true);
        AddBotButton.SetActive(false);
    }
}
