using UnityEngine;

public class PlayerController : Photon.MonoBehaviour {
    public const string Opponent = "Opponent";
    public const string Player = "Player";
    public Color PlayerColor = Color.blue;
    public Color OpponentColor = Color.red;

    public int ID { get; protected set; }
	public bool isDummy=false;
    public RobotStateMachine RobotStateMachine { get; protected set; }


    [HideInInspector] public PlayerHealth PlayerHealth;
    [HideInInspector] public PlayerPower PlayerPower;
    [HideInInspector] public PlayerPhysics PlayerPhysics = null;
    [HideInInspector] public Animator Animator = null;
    [HideInInspector] public GameObject PlayerInfo;
    [HideInInspector] public GameObject Canvas;
    [HideInInspector] public GameObject OpponentInfo;

    protected bool IsInitialized = false;

    void Start() {
        this.Initialize();
        this.AddPlayerToGame();
        this.IsInitialized = true;
    }

    protected virtual void AddPlayerToGame() {
        GameManager.Instance.AddPlayerToGame(this);
    }

    void Update() {
    }

    protected virtual void SetEntity() {
		if (!photonView.isMine || isDummy) {
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

        this.GetComponent<RobotAutomaton>().enabled = true;
        this.GetComponentInChildren<MeshRenderer>().material.color = 
            this.PlayerColor;
        TargetManager.instance.SetPlayer(gameObject);
        this.PlayerInfo = this.Canvas.transform.GetChild(1).gameObject;
        this.PlayerInfo.SetActive(true);
    }
		
    protected virtual void SetOpponent() {
        this.SetTag(Opponent);
        TargetManager.instance.AddOpponent(gameObject);
        this.GetComponentInChildren<MeshRenderer>().material.color =
             this.OpponentColor;
        this.OpponentInfo = this.Canvas.transform.GetChild(0).gameObject;
    }

    public virtual void UpdateAnimations(string animationName) {
        if (!this.photonView.isMine) return;

        this.Animator.SetTrigger(animationName);
    }

    public virtual void UpdateDeadToOthers() {
        this.photonView.RPC("ReceiveDeadFromOthers", PhotonTargets.Others,
            this.ID);
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

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (!this.IsInitialized) return;

        if (stream.isWriting) {
            stream.SendNext(this.PlayerHealth.Health);
            stream.SendNext(this.PlayerPower.Power);
        } else {
            this.PlayerHealth.Health = (int)stream.ReceiveNext();
            this.PlayerPower.Power = (int)stream.ReceiveNext();
        }
    }
}
