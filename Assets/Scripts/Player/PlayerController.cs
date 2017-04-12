using UnityEngine;
using System.Collections;
using System.Reflection;
using System;

public class PlayerController : Photon.MonoBehaviour {
    public const string Opponent = "Opponent";
    public const string Player = "Player";
    public Color PlayerColor = Color.blue;
    public Color OpponentColor = Color.red;
	public int powerRecoverySpeed = 5;
	public float timeBetweenPowerRecovery = 1.0f;

    public int ID { get; protected set; }
    public RobotStateMachine RobotStateMachine { get; protected set; }

    [HideInInspector] public PlayerHealth PlayerHealth;
    [HideInInspector] public PlayerPower PlayerPower;
    [HideInInspector] public PlayerPhysics PlayerPhysics = null;
    [HideInInspector] public PlayerAudio PlayerAudio;
    [HideInInspector] public Animator Animator = null;
    [HideInInspector] public GameObject PlayerInfo;
    [HideInInspector] public GameObject Canvas;
	[HideInInspector] public GameObject OpponentInfo;
	[HideInInspector] public InputManager inputManager;
	[HideInInspector] public GameObject Shield;
	[HideInInspector] public GameObject Lightnings;
	[HideInInspector] public TargetManager TargetManager;


    void Awake() {
        this.Initialize();
        this.AddPlayerToGame();
    }

    protected virtual void AddPlayerToGame() {
        GameManager.Instance.AddPlayerToGame(this);
    }

    void Update() {
    }

    protected virtual void SetEntity() {
		
		if (GameManager.Instance.LocalPlayer != null  || !photonView.isMine) {
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
        this.PlayerAudio = GetComponent<PlayerAudio>();
        this.Animator = GetComponent<Animator>();
        this.Canvas = GameObject.FindGameObjectWithTag("Canvas");
        this.ID = this.photonView.viewID;
        this.PlayerHealth = new PlayerHealth(this);
        this.PlayerPower = new PlayerPower(this);
		this.inputManager = gameObject.GetComponent<InputManager>();
		this.Lightnings = transform.FindChild ("Lightnings").gameObject;
		this.Shield = transform.FindChild ("Shield").gameObject;
		this.TargetManager = gameObject.GetComponent<TargetManager>();

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

        this.GetComponent<RobotAutomaton>().enabled = true;
        /*this.GetComponentInChildren<MeshRenderer>().material.color = 
            this.PlayerColor;*/
        //TargetManager.instance.SetPlayer(gameObject);
        this.PlayerInfo = this.Canvas.transform.GetChild(1).gameObject;
        this.PlayerInfo.SetActive(true);
		StartCoroutine(recoverPower());
    }
		
	IEnumerator recoverPower()
	{
		while(PlayerHealth.Health>0) {
			this.PlayerPower.Power += powerRecoverySpeed;
			yield return new WaitForSeconds(timeBetweenPowerRecovery);
		}
	}   
		
    protected virtual void SetOpponent() {
        this.SetTag(Opponent);
        //TargetManager.instance.AddOpponent(gameObject);
        /*this.GetComponentInChildren<MeshRenderer>().material.color =
             this.OpponentColor;*/
        this.OpponentInfo = this.Canvas.transform.GetChild(0).gameObject;
		if (gameObject.GetComponent<AI> () != null) {
			StartCoroutine(recoverPower());
		}
    }

    public virtual void UpdateAnimations(string animationName) {
        if (!this.photonView.isMine) return;

        this.Animator.SetTrigger(animationName);
    }

    public virtual void UpdateDeadToOthers() {
        this.photonView.RPC("ReceiveDeadFromOthers", PhotonTargets.Others,
            this.ID);
    }

    public virtual void UpdateAudioToOthers(string audioName)
    {
        this.photonView.RPC("ReceiveAudioFromOthers", PhotonTargets.Others, this.ID, audioName);
    }

    [PunRPC]
    public virtual void ReceiveDeadFromOthers(int playerID) {
        GameManager.Instance.UpdateDeadList(playerID);
    }

    [PunRPC]
    public virtual void SendAnimations(string animationName) {
        if (this.Animator == null) return;

        this.Animator.SetTrigger(animationName);
    }

    [PunRPC]
    public virtual void ReceiveAudioFromOthers(int playerID, string audioName)
    {
        PlayerAudio audio = GameManager.Instance.PlayerList[playerID].PlayerController.PlayerAudio;
        Type audioType = audio.GetType();
        MethodInfo theMethod = audioType.GetMethod(audioName);
        theMethod.Invoke(audio, null);
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting) {
            stream.SendNext(this.PlayerHealth.Health);
            stream.SendNext(this.PlayerPower.Power);
        } else {
            this.PlayerHealth.Health = (int)stream.ReceiveNext();
            this.PlayerPower.Power = (int)stream.ReceiveNext();
        }
    }
}
