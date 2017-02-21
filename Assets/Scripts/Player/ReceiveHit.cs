using UnityEngine;

public class ReceiveHit : MonoBehaviour {
	void OnCollisionEnter (Collision other) {
	    if (!this.CheckIfValid(other)) return;
	    this.HandleOpponent(other);
	    this.HandlePlayer(other);
	}

    protected virtual bool CheckIfValid(Collision other) {
        return other.transform.name.Equals("Robot:SwordRight") || 
            other.transform.name.Equals("Robot:SwordLeft");
    }

    protected virtual void HandlePlayer(Collision other) {
        if (!other.gameObject.CompareTag(PlayerController.Player)) return;
    }

    protected virtual void HandleOpponent(Collision other) {
        if (!other.gameObject.CompareTag(PlayerController.Opponent)) return;

        PhotonNetwork.player.TakeDamage(10);
    }
}
