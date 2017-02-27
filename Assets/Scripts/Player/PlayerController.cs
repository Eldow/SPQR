using UnityEngine;

public class PlayerController : MonoBehaviour {
    public const string Player = "Player";
    public Color PlayerColor = Color.blue;

    [HideInInspector]
    public PlayerHealth PlayerHealth = null;
    [HideInInspector]
    public PlayerPower PlayerPower = null;
    [HideInInspector]
    public PlayerPhysics PlayerPhysics = null;
    [HideInInspector]
    public Animator Animator = null;
    [HideInInspector]
    public GameObject PlayerInfo;
    [HideInInspector]
    public GameObject Canvas;

    protected virtual void SetTag(string tagName){
        transform.tag = tagName;
        Transform[] temp = GetComponentsInChildren<Transform>();

        foreach (Transform t in temp) {
            t.tag = tagName;
        }
    }

    protected virtual void Initialize() {
        this.PlayerHealth = GetComponent<PlayerHealth>();
        this.PlayerPower = GetComponent<PlayerPower>();
        this.PlayerPhysics = GetComponent<PlayerPhysics>();
        this.Animator = GetComponent<Animator>();
        this.Canvas = GameObject.FindGameObjectWithTag("Canvas");

        this.SetEntity();
    }

    void Start() {
        this.Initialize();
    }

    protected virtual void SetEntity() {
            this.SetPlayer();
    }

    protected virtual void SetPlayer() {
        this.SetTag(Player);
        GetComponent<RobotAutomaton>().enabled = true;
        GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
        TargetManager.instance.SetPlayer(gameObject);
        PlayerInfo = Canvas.transform.GetChild(1).gameObject;
        PlayerInfo.SetActive(true);
        PhotonNetwork.player.SetHealth(PlayerHealth.MaxHealth);
        PhotonNetwork.player.SetPower(PlayerPower.MaxPower);
    }

    public virtual void UpdateAnimations(string animationName) {
        this.Animator.SetTrigger(animationName);
    }
}