using UnityEngine;

public class ReceiveHit : Photon.MonoBehaviour {
	//Can handle own collider
	void OnCollisionEnter (Collision other)
	{
		//Use for different players
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

        HealthManager healthManager =
            TargetManager.instance.player.GetComponent<HealthManager>();

        healthManager.TakeDamage(10); // TO FIX
    }

    protected virtual void HandleOpponent(Collision other) {
        if (!other.gameObject.CompareTag(PlayerController.Opponent)) return;


    }
}
