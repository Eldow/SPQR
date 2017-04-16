using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchmakingPanelBehaviour : MonoBehaviour {

    public GameObject PlayButton;
    public GameObject LeaveButton;
    public GameObject AddBotButton;
    public GameObject MapDropdown;
    public GameObject ModeDropdown;
    
    void Start()
    {
        OutOfFriendRoom();
    }

    public void OutOfFriendRoom()
    {
        PlayButton.SetActive(true);
        LeaveButton.SetActive(false);
        AddBotButton.SetActive(true);
        MapDropdown.GetComponent<Dropdown>().interactable = true;
        ModeDropdown.GetComponent<Dropdown>().interactable = true;
    }

    public void InFriendRoom()
    {
        PlayButton.SetActive(false);
        LeaveButton.SetActive(true);
        AddBotButton.SetActive(false);
        MapDropdown.GetComponent<Dropdown>().interactable = false;
        ModeDropdown.GetComponent<Dropdown>().interactable = false;
    }
}
