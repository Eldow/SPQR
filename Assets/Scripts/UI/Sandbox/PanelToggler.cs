using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelToggler : MonoBehaviour {

    public GameObject ScorePanel;
    public GameObject MenuPanel;
    public GameObject Trash;
    private InputManager _inputManager;
    private bool doItOnce;
    private bool _isMenuActive = false;

    void Start()
    {
        _inputManager = GetComponent<InputManager>();
        if(ScorePanel != null)ScorePanel.transform.parent = Trash.transform;
		doItOnce = false;
        MenuPanel.SetActive(false);
    }
    void Update()
    {
        if (_inputManager.infoButton() && ScorePanel != null)
        {
            if (ScorePanel.transform.parent.name.Equals("Canvas"))
            {
                ScorePanel.transform.parent = Trash.transform;

            } else
            {
                ScorePanel.transform.parent = transform;
            }
        }
        if (_inputManager.menuButton())
        {
            _isMenuActive = !_isMenuActive;
            MenuPanel.SetActive(_isMenuActive);
        }
        if (GameManager.Instance != null && GameManager.Instance.isCompletingRound && !doItOnce) {
			ScorePanel.transform.parent = transform;
			doItOnce = true;
		}
    }
}
