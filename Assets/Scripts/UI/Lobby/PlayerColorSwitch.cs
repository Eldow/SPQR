using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PlayerColorSwitch : MonoBehaviour, IPointerClickHandler
{

    private Image _backgroundImage;
    private int _index = 1;
	// Use this for initialization
	void Start () {
        _backgroundImage = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetPlayerColor(int index)
    {
        _backgroundImage = GetComponent<Image>();
        switch (index)
        {
            case 1:
                _backgroundImage.color = Color.white;
                break;
            case 2:
                _backgroundImage.color = Color.black;
                break;
            case 3:
                _backgroundImage.color = Color.blue;
                break;
            case 4:
                _backgroundImage.color = Color.red;
                break;
            case 5:
                _backgroundImage.color = Color.green;
                break;
            case 6:
                _backgroundImage.color = Color.yellow;
                break;
            case 7:
                _backgroundImage.color = Color.magenta;
                break;
            case 8:
                _backgroundImage.color = Color.cyan;
                break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        bool isBot = false;
        ChatManager chat = GameObject.Find("ChatManager").GetComponent<ChatManager>();
        string text = transform.parent.FindChild("Text").GetComponent<Text>().text;

        if (chat.IsMaster() && text.Contains("Bot")) isBot = true;
        if (!text.Equals(PhotonNetwork.playerName) && !isBot) return;

        _index++;
        if (_index > 8) _index = 1;
        SetPlayerColor(_index);
        if (!isBot)
        {
            chat.SendModifyTeam(_index);
        } else
        {
            chat.SendModifyBot(text, _index);
        }
    }
}
