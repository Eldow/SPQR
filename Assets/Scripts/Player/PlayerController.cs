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
        OpponentInfo = Canvas.transform.GetChild(0).gameObject;
        this.SetTag(Opponent);
    }

    public virtual void UpdateAnimations(string animationName) {
        this.Animator.SetTrigger(animationName);
        photonView.RPC(
            "SendAnimations",
            PhotonTargets.Others,
            animationName
        );
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
        if (stream.isWriting) {
            stream.SendNext(this.PlayerHealth.Health);
            stream.SendNext(this.PlayerPower.Power);
        } else {
            this.PlayerHealth.Health = (int)stream.ReceiveNext();
            this.PlayerPower.Power = (int)stream.ReceiveNext();
        }
    }
}
