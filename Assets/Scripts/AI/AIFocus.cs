using UnityEngine;

public class AIFocus : MonoBehaviour {   

	private TargetManager targetManager;

	
	void Start() {
		if (!PhotonNetwork.isMasterClient)
			Destroy (this.GetComponent<AIFocus> ());
		targetManager = gameObject.GetComponent<PlayerController> ().TargetManager;
	}
	
	void Update() {	

        Quaternion neededRotation;
		if (targetManager.currentTarget != null) {
            neededRotation = Quaternion.LookRotation(
				targetManager.currentTarget.transform.position - 
                gameObject.transform.position
            );
        } else {
			targetManager.updateNearestOpponent ();
            neededRotation = Quaternion.LookRotation(
                gameObject.transform.forward,
                gameObject.transform.up
            );
        }

        gameObject.transform.rotation = Quaternion.Slerp(
            gameObject.transform.rotation, 
            neededRotation, 
            Time.deltaTime * 5f
        );

	}

}