using UnityEngine;

public class HandleHit : Photon.MonoBehaviour {
    protected PlayerController PlayerController = null;

    void Start() {
        this.PlayerController 
            = this.transform.root.GetComponent<PlayerController>();
    }

    void OnCollisionEnter(Collision other) {
        if (!this.CheckIfValid(other)) return;

        this.HandleOpponent(other);
        this.HandlePlayer(other);
    }

    protected virtual bool CheckIfValid(Collision other) {
        return this.photonView.isMine && 
            this.transform.root.CompareTag(PlayerController.Player);
    }

    protected virtual void HandleOpponent(Collision other) {
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
			
        robotAttackState.HandleAttack(this, other);
    }

    public virtual void SendHit(Collision other, int damage, int hitstun) {
        int opponentID = -1;

        if ((opponentID = this.GetOpponentID(other)) == -1) {
            return;
        }

        this.photonView.RPC("ReceiveHit", PhotonTargets.AllViaServer, damage, 
            hitstun, opponentID);
    }

    protected virtual void HandlePlayer(Collision other) {
        if (!other.transform.root.CompareTag(PlayerController.Player)) return;
    }

    protected virtual void SendHitstun(PlayerController who, int hitstun) {
		if(who!=null)
        who.RobotStateMachine.SetState(new RobotHitstunState(hitstun));
    }

    protected virtual int GetOpponentID(Collision other) {
        PlayerController opponentController = 
            other.transform.root.GetComponent<PlayerController>();

        if (opponentController == null) {
            return -1;
        }

        return opponentController.ID;
    }

    [PunRPC]
    public void ReceiveHit(int damage, int hitstun, int playerID) {
        /* Used once per client, so we need to send the hit to the right 
         * Robot! */
        PlayerController who = 
            GameManager.Instance.PlayerList[playerID].PlayerController;

        who.PlayerHealth.Health -= damage;
        this.SendHitstun(who, hitstun);
    }
}