using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FiltersBehaviour : MonoBehaviour {

    private ChatManager _chatManager;

    void Start()
    {
        _chatManager = GameObject.Find("ChatManager").GetComponent<ChatManager>();
    }

	public void MapChanged(int map)
    {
        _chatManager.SendChangeMap(map);
    }

    public void ModeChanged(int mode)
    {
        _chatManager.SendChangeMode(mode);
    }

    public void SetMap(int map)
    {
        transform.FindChild("MapLabel/MapDropdown").GetComponent<Dropdown>().value = map;
    }

    public void SetMode(int mode)
    {
        transform.FindChild("ModeLabel/Mode Dropdown").GetComponent<Dropdown>().value = mode;
    }
}
