using UnityEngine;

public class AIFocus : MonoBehaviour {   

	private GameObject player;

	void FindPlayer() {
		if (TargetManager.instance == null) return;
        GameObject player = TargetManager.instance.player;
	}
	
	void Start() {
		FindPlayer();
	}
	
	void Update() {	

        Quaternion neededRotation;

        if (player != null) {
            neededRotation = Quaternion.LookRotation(
                player.transform.position - 
                gameObject.transform.position
            );
        } else {
			FindPlayer();
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