using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(InputField))]
public class PlayerNameInputField : MonoBehaviour
{

    static string playerNamePrefKey = "PlayerName";
    InputField _inputField;

    void Start()
    {
        string defaultName = "";
        _inputField = this.GetComponent<InputField>();
        if (_inputField != null)
        {
            if (PlayerPrefs.HasKey(playerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                _inputField.text = defaultName;
            }
        }
        PhotonNetwork.playerName = defaultName;
    }

    public void SetPlayerName(string value)
    {
        PhotonNetwork.playerName = value;
        PlayerPrefs.SetString(playerNamePrefKey, value);
    }
}
