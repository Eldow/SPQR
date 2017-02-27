using UnityEngine;

public class NetPlayerController : PlayerController {
    public const string Opponent = "Opponent";
        
    public Color OpponentColor = Color.red;

    [HideInInspector]
    public PhotonView PhotonView;
    [HideInInspector]
    public GameObject OpponentInfo;

    void Start () {
        this.Initialize();
    }

	void Update () {

    }

    protected override void Initialize() {
        this.SetPhotonView();
        base.Initialize();
    }

    protected virtual void SetPhotonView() {
        this.PhotonView = this.gameObject.GetComponent<PhotonView>();
    }

    protected override void SetEntity() {
        if (!this.PhotonView.isMine) {
            this.SetOpponent();
        } else {
            this.SetPlayer();
        }
    }

    protected virtual void SetOpponent() {
        TargetManager.instance.AddOpponent(gameObject);
        OpponentInfo = Canvas.transform.GetChild(0).gameObject;
        this.SetTag(Opponent);
    }

    public override void UpdateAnimations(string animationName) {
        base.UpdateAnimations(animationName);
        this.PhotonView.RPC(
            "SendAnimations", 
            PhotonTargets.Others, 
            animationName
        );
    }

    [PunRPC]
    void SendAnimations(string animationName) {
        if (this.Animator == null) return;

        this.Animator.SetTrigger(animationName);
    }
}
