using System;
using UnityEngine;

public class PlayerController : Photon.MonoBehaviour {
    public const string Opponent = "Opponent";
    public const string Player = "Player";
    public Color PlayerColor = Color.blue;
    public Color OpponentColor = Color.red;
    public int ID { get; protected set; }

    [HideInInspector]
    public PlayerHealth PlayerHealth;
    [HideInInspector]
    public PlayerPower PlayerPower;
    [HideInInspector]
    public PlayerPhysics PlayerPhysics = null;
    [HideInInspector]
    public Animator Animator = null;
    [HideInInspector]
    public GameObject PlayerInfo;
    [HideInInspector]
    public GameObject Canvas;

    [HideInInspector]
    public GameObject OpponentInfo;

    public RobotStateMachine RobotStateMachine { get; protected set; }

    void Start() {
        this.Initialize();
        this.AddPlayerToGame();
        if (PhotonNetwork.player.IsMasterClient)
        {
            GameManager.Instance.MasterPlayer = this;
        }
    }

    protected virtual void AddPlayerToGame() {
        GameManager.Instance.AddPlayerToGame(this);
    }

    void Update() {

    }

    protected virtual void SetEntity() {
        if (!photonView.isMine) {
            this.SetOpponent();
        } else {
            this.SetPlayer();
        }
    }

    protected virtual void SetTag(string tagName) {
        transform.tag = tagName;
        Transform[] temp = GetComponentsInChildren<Transform>();

        foreach (Transform t in temp) {
            t.tag = tagName;
        }
    }

    protected virtual void Initialize() {
        this.PlayerPhysics = GetComponent<PlayerPhysics>();
        this.Animator = GetComponent<Animator>();
        this.Canvas = GameObject.FindGameObjectWithTag("Canvas");
        this.ID = this.photonView.viewID;
        this.PlayerHealth = new PlayerHealth(this);
        this.PlayerPower = new PlayerPower(this);
        RobotAutomaton robotAutomaton = this.GetComponent<RobotAutomaton>();

        if (robotAutomaton != null && 
            robotAutomaton.StateMachine is RobotStateMachine) {
            this.RobotStateMachine = 
                (RobotStateMachine)robotAutomaton.StateMachine;
        }

        this.SetEntity();
    }

    protected virtual void SetPlayer() {
        this.SetTag(Player);
        GetComponent<RobotAutomaton>().enabled = true;
        GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
        TargetManager.instance.SetPlayer(gameObject);
        PlayerInfo = Canvas.transform.GetChild(1).gameObject;
        PlayerInfo.SetActive(true);
    }

    protected virtual void SetOpponent() {
        TargetManager.instance.AddOpponent(gameObject);
        OpponentInfo = Canvas.transform.GetChild(1).gameObject;
        this.SetTag(Opponent);
    }

    public virtual void UpdateAnimation(int playerId, string animationName) {
        if (this.Animator == null) return;

        GameManager.Instance.PlayersList[playerId]
            .Animator.SetTrigger(animationName);
    }

    public virtual void UpdateState(string stateName) {
        this.SendStateToMaster(this.ID, stateName);
    }

    /************************** CLIENT **************************/
    public virtual void SendHealthToMaster() {
        if (!photonView.isMine) return;

        this.photonView.RPC(
            "ReceiveHealthFromOther",
            PhotonTargets.MasterClient,
            this.ID,
            this.PlayerHealth.Health);
    }

    public virtual void SendStateToMaster(int playerID, string stateName) {
        Debug.Log("CLIENT sends: " + playerID + " " + stateName);
        this.photonView.RPC(
            "ReceiveStateFromOther", 
            PhotonTargets.MasterClient,
            playerID, 
            stateName);
    }

    [PunRPC]
    public virtual void ReceiveStateFromMaster(int playerID, 
        string stateName) {
        Debug.Log("CLIENT receives: " + playerID + " " + stateName);
        Type type = Type.GetType(stateName);

        if (type == null) return;

        RobotState robotState = 
            (RobotState)Activator.CreateInstance(type);

        GameManager.Instance.PlayersList[playerID].RobotStateMachine
            .SetState(robotState);

        this.UpdateAnimation(playerID, stateName);
    }

    [PunRPC]
    public virtual void ReceiveHealthFromMaster(int playerID, int health) {
        GameManager.Instance.UpdatePlayerHealth(playerID, health);
    }

    /************************** MASTER **************************/
    public virtual void SendStateToOthers(int playerID, string stateName) {
        Debug.Log("MASTER sends: " + playerID + " " + stateName);
        this.photonView.RPC(
            "ReceiveStateFromMaster", 
            PhotonTargets.AllViaServer,
            playerID, 
            stateName);
    }

    public virtual void ReceiveHealthFromOther(int playerID, int health) {
        GameManager.Instance.UpdatePlayerHealth(playerID, health);
    }

    [PunRPC]
    public virtual void ReceiveStateFromOther(int playerID, string stateName) {
        Debug.Log("MASTER receives: " + playerID + " " + stateName);
        this.photonView.RPC(
            "ReceiveStateFromMaster", 
            PhotonTargets.AllViaServer,
            playerID, 
            stateName);
    }

    void OnPhotonSerializeView() {
        
    }
}
