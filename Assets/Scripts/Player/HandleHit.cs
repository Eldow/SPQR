using UnityEngine;

public class HandleHit : Photon.MonoBehaviour {
    protected PlayerController PlayerController = null;

    void Start() {
        this.PlayerController 
            = this.gameObject.GetComponent<PlayerController>();
    }

    void OnCollisionEnter (Collision other) {
        if (!this.CheckIfValid(other)) return;

        this.HandleOpponent(other);
        this.HandlePlayer(other);
    }

    protected virtual bool CheckIfValid(Collision other) {
        return other.transform.name.Equals("Robot:SwordRight") || 
            other.transform.name.Equals("Robot:SwordLeft");
    }

    protected virtual void HandleOpponent(Collision other) {
        if (!other.gameObject.CompareTag(PlayerController.Player)) return;

        PhotonView photonView = PhotonView.Get(this);

        if (!(this.PlayerController.RobotStateMachine.CurrentState is 
            RobotAttackState)) {
            return;
        }

        RobotAttackState robotAttackState = 
            (RobotAttackState)this.PlayerController.RobotStateMachine
            .CurrentState;

        if (!HandleHit.IsAttackActive(robotAttackState)) return;

        photonView.RPC("GetHit", PhotonTargets.All, 
            robotAttackState.Damage, robotAttackState.Hitstun,
            this.PlayerController.ID);
    }

    public static bool IsAttackActive(RobotAttackState robotAttackState) {
        return robotAttackState.CurrentFrame >= 
            robotAttackState.MinActiveState && 
            robotAttackState.CurrentFrame <= 
            robotAttackState.MaxActiveState;
    }

    protected virtual void HandlePlayer(Collision other) {
        if (!other.gameObject.CompareTag(PlayerController.Opponent)) return;
    }

    protected virtual void SendHitstun(int hitstun) {
        this.PlayerController.RobotStateMachine.SetState(
            new RobotHitstunState(hitstun));
    }

    protected virtual int GetID(Collision other) {
        PlayerController opponentController = 
            other.transform.root.GetComponent<PlayerController>();

        if (opponentController == null) {
            return -1;
        }

        return opponentController.ID;
    }

    [PunRPC]
    public void GetHit(int damage, int hitstun, int playerID) {
        if (playerID != this.PlayerController.ID) return;

        this.PlayerController.PlayerHealth.Health -= damage;
        this.SendHitstun(hitstun);
    }
}