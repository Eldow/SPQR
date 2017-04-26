using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEntryBehaviour : MonoBehaviour {

    private ChatManager _chatManager;
    public GameObject DeleteButton;

	void Start()
    {
        _chatManager = GameObject.Find("ChatManager").GetComponent<ChatManager>();
        SetVisibility();
    }

    public void SetVisibility()
    {
        if (_chatManager.IsMaster() && !transform.Find("Text").GetComponent<Text>().text.Equals(PhotonNetwork.playerName))
        {
            DeleteButton.SetActive(true);
        } else
        {
            DeleteButton.SetActive(false);
        }
    }

    public void KickPlayer()
    {
        SoundManager.instance.PlayClick();
        _chatManager.KickPlayer();
    }
}
