using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelToggler : MonoBehaviour {

    private InputManager _inputManager;
    public GameObject ScorePanel;
    public GameObject MenuPanel;

    void Start()
    {
        _inputManager = GetComponent<InputManager>();
    }
    void Update()
    {
        if (_inputManager.infoButton())
        {
            ScorePanel.SetActive(!ScorePanel.activeInHierarchy);
        }
    }
}
