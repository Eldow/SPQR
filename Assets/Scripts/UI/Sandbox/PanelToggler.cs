using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelToggler : MonoBehaviour {

    private InputManager _inputManager;
    public GameObject ScorePanel;
    public GameObject MenuPanel;
    public GameObject Trash;

    void Start()
    {
        _inputManager = GetComponent<InputManager>();
        ScorePanel.transform.parent = Trash.transform;

    }
    void Update()
    {
        if (_inputManager.infoButton())
        {
            if (ScorePanel.transform.parent.name.Equals("Canvas"))
            {
                ScorePanel.transform.parent = Trash.transform;

            } else
            {
                ScorePanel.transform.parent = transform;
            }
        }
    }
}
