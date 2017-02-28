public class NetPlayerPhysics : PlayerPhysics {
    private PhotonView _photonView = null;

    void Start () {
        this.Initialize();
    }

    void Update () {
        this.UpdatePhysics();
    }

    protected override void Initialize() {
        base.Initialize();
        this._photonView = this.gameObject.GetPhotonView();
    }

    public override void Movement(float speedFactor = 1.0f) {
        this.IsMoving = true;

        if (!this._photonView.isMine) return;

        base.Movement(speedFactor);
    }
}
