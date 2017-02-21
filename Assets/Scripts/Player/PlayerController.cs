using UnityEngine;
using UnityEngine.Networking;
/*

    This class manages the player's behaviour

*/
public class PlayerController : Photon.MonoBehaviour {
    public const string Player = "Player";
    public const string Opponent = "Opponent";

    [HideInInspector]
    public HealthManager HealthManager = null;
    [HideInInspector]
    public PowerManager PowerManager = null;
    [HideInInspector]
    public PlayerPhysics PlayerPhysics = null;
    [HideInInspector]
    public Animator Animator = null;
    [HideInInspector]
    public GameObject playerInfo;
    [HideInInspector]
    public GameObject opponentInfo;
    [HideInInspector]
    public GameObject Canvas;

    public Color PlayerColor = Color.blue;
    public Color OpponentColor = Color.red;

    private void SetTag(string tagName){
		Transform[] temp;
		transform.tag = tagName;
		temp = GetComponentsInChildren<Transform>();

		foreach (Transform t in temp) {
			t.tag = tagName;
		}
	}

    // On Opponent spawn
    void Start() {
        this.HealthManager = GetComponent<HealthManager>();
        this.PowerManager = GetComponent<PowerManager>();
        this.PlayerPhysics = GetComponent<PlayerPhysics>();
        this.Animator = GetComponent<Animator>();
        this.Canvas = GameObject.FindGameObjectWithTag("Canvas");

        if (!photonView.isMine) {
            this.SetOpponent();
        } else {
            this.SetPlayer();
        }
    }

    protected virtual void SetOpponent() {
        TargetManager.instance.AddOpponent(gameObject);
        opponentInfo = Canvas.transform.GetChild(0).gameObject;
        this.SetTag(Opponent);
    }

    protected virtual void SetPlayer() {
        SetTag(Player);
        GetComponent<RobotAutomaton>().enabled = true;
        GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
        TargetManager.instance.SetPlayer(gameObject);
        playerInfo = Canvas.transform.GetChild(1).gameObject;
        playerInfo.SetActive(true);
    }

    public virtual void UpdateAnimations(string animationName) {
        this.Animator.SetTrigger(animationName);
        photonView.RPC("SendAnimations", PhotonTargets.Others, animationName);
    }

    [PunRPC]
    void SendAnimations(string animationName) {
        this.Animator.SetTrigger(animationName);
    }
}