using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchmakingPanelBehaviour : MonoBehaviour {

    public GameObject PlayButton;
    public GameObject LeaveButton;
    
    void Start()
    {
        OutOfFriendRoom();
    }

    public void OutOfFriendRoom()
    {
        PlayButton.SetActive(true);
        LeaveButton.SetActive(false);
    }

    public void InFriendRoom()
    {
        PlayButton.SetActive(false);
        LeaveButton.SetActive(true);
    }
}
