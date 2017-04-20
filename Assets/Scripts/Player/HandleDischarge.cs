using UnityEngine;

public class HandleDischarge : Photon.MonoBehaviour {
    protected PlayerController PlayerController = null;

    void Start() {
        this.PlayerController 
            = this.transform.root.GetComponent<PlayerController>();
    }

    protected virtual bool CheckIfValid(Collider other) {
		return this.photonView.isMine && other.transform.root.tag.Equals(PlayerController.Opponent);
    }

    void OnTriggerStay(Collider other) {
        if (!this.CheckIfValid(other)) return;

        if (this.PlayerController == null || !(this.PlayerController.RobotStateMachine.CurrentState is
            RobotAttackState)) {
            return;
        }

        RobotAttackState robotAttackState =
            (RobotAttackState)this.PlayerController.RobotStateMachine
                .CurrentState;

        robotAttackState.HandleAttackTrigger(this, other);
    }

    protected virtual void HandlePlayerTrigger(Collider other) {
        if (!other.transform.root.CompareTag(PlayerController.Player)) return;
    }

    protected virtual void HandleOpponentTrigger(Collider other) {
        if (!other.transform.root.CompareTag(PlayerController.Opponent)) {
            return;
        }

        if (!(this.PlayerController.RobotStateMachine.CurrentState is
            RobotAttackState)) {
            return;
        }

        RobotAttackState robotAttackState =
            (RobotAttackState)this.PlayerController.RobotStateMachine
            .CurrentState;

        robotAttackState.HandleAttackTrigger(this, other);
    }

    public virtual void SendPoke(GameObject other, Vector3 direction) {
        int opponentID = -1;

        if ((opponentID = this.GetOpponentID(other)) == -1) {
            return;
        }

        this.photonView.RPC("ReceivePoke", PhotonTargets.Others, 
            direction, opponentID);
    }

    protected virtual int GetOpponentID(GameObject other) {
        PlayerController opponentController = 
            other.transform.root.GetComponent<PlayerController>();

        if (opponentController == null) {
            return -1;
        }

        return opponentController.ID;
    }

    public virtual void SendHit(GameObject other, int damage, int hitstun) {
        int opponentID = -1;

        if ((opponentID = this.GetOpponentID(other)) == -1) {
            return;
        }

        this.photonView.RPC("ReceiveHit", PhotonTargets.AllViaServer, damage,
            hitstun, opponentID);
    }

    [PunRPC]
    public void ReceivePoke(Vector3 direction, int playerID) {
        /* Used once per client, so we need to send the hit to the right 
         * Robot! */

        PlayerController who =
            GameManager.Instance.PlayerList[playerID].PlayerController;

		if (who==null || !who.photonView.isMine)
			return;
		
        who.PlayerPhysics.ApplyPoke(direction);
    }

    [PunRPC]
    public void ReceiveHit(int damage, int hitstun, int playerID) {
        /* Used once per client, so we need to send the hit to the right 
         * Robot! */
        PlayerController who =
            GameManager.Instance.PlayerList[playerID].PlayerController;

        if (who == null || !who.photonView.isMine)
            return;

        who.PlayerHealth.Health -= damage;
        this.SendHitstun(who, hitstun);
    }

    protected virtual void SendHitstun(PlayerController who, int hitstun) {
        if (who != null)
            who.RobotStateMachine.SetState(new RobotHitstunState(hitstun));
    }
}