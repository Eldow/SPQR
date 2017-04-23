using UnityEngine;

public class HandleHit : Photon.MonoBehaviour {
    protected PlayerController PlayerController = null;

    void Start() {
        this.PlayerController 
            = this.transform.root.GetComponent<PlayerController>();
    }

    void OnCollisionEnter(Collision other) {
        if (!this.CheckIfValid()) return;

        if (this.transform.root == other.transform.root) return;

		if(!other.transform.root.tag.Equals (PlayerController.Opponent) && !other.transform.root.tag.Equals (PlayerController.Player))
			return;

		if (this.PlayerController==null || !(this.PlayerController.RobotStateMachine.CurrentState is 
			RobotAttackState)) {
			return;
		}

		RobotAttackState robotAttackState = 
			(RobotAttackState)this.PlayerController.RobotStateMachine
				.CurrentState;

		robotAttackState.HandleAttack(this, other);

       // this.HandleOpponent(other);
       // this.HandlePlayer(other);
    }


    protected virtual bool CheckIfValid() {
		return this.photonView.isMine;
    }

    protected virtual void HandleOpponent(Collision other) {
        if (!other.transform.root.CompareTag(PlayerController.Opponent)) {
            return;
        }

        if (this.PlayerController == null || !(this.PlayerController.RobotStateMachine.CurrentState is
                RobotAttackState)) {
            return;
        }

        RobotAttackState robotAttackState =
            (RobotAttackState) this.PlayerController.RobotStateMachine
                .CurrentState;

        robotAttackState.HandleAttack(this, other);
    }

    public virtual void SendHit(GameObject other, int damage, int hitstun) {
        int opponentID = -1;

        if ((opponentID = this.GetOpponentID(other)) == -1) {
            return;
        }

        this.photonView.RPC("ReceiveHit", PhotonTargets.AllViaServer, damage, 
            hitstun, opponentID);
    }

    protected virtual void HandlePlayer(Collision other) {
		if (!PhotonNetwork.offlineMode)
			return;

		if (!other.transform.root.CompareTag (PlayerController.Player)) {
			return;
		}
		if (this.PlayerController == null || !(this.PlayerController.RobotStateMachine.CurrentState is 
			RobotAttackState)) {
			return;
		}
		RobotAttackState robotAttackState = 
			(RobotAttackState)this.PlayerController.RobotStateMachine
				.CurrentState;

		robotAttackState.HandleAttack (this, other);
	}

    protected virtual void SendHitstun(PlayerController who, int hitstun) {
		if(who!=null)
        who.RobotStateMachine.SetState(new RobotHitstunState(hitstun));
    }

    protected virtual int GetOpponentID(GameObject other) {
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

		if (who==null || !who.photonView.isMine)
			return;

        who.PlayerHealth.Health -= damage;
        this.SendHitstun(who, hitstun);
    }
}