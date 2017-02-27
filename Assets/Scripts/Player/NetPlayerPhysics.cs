public class NetPlayerPhysics : PlayerPhysics {
    private PhotonView PhotonView = null;

    void Start () {
        this.Initialize();
    }

    void Update () {
        this.UpdatePhysics();
    }

    protected override void Initialize() {
        base.Initialize();
        this.PhotonView = this.gameObject.GetPhotonView();
    }

    public override void Movement(float speedFactor = 1.0f) {
        this.IsMoving = true;

        if (!this.PhotonView.isMine) return;

        base.Movement(speedFactor);
    }
}
