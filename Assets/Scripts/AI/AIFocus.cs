using UnityEngine;

public class AIFocus : MonoBehaviour {   

	private TargetManager targetManager;
	private PlayerController pc;

	void Start()
	{
		pc = gameObject.GetComponent<PlayerController> ();
		targetManager = pc.TargetManager;
		if (!pc.isAI)
			Destroy (this);
	}


	void Update() {	

		if (!PhotonNetwork.isMasterClient)
			return;
		
		if (!pc.isAI)
			Destroy (this);
		
        Quaternion neededRotation;
		if (targetManager.currentTarget != null) {
            neededRotation = Quaternion.LookRotation(
				targetManager.currentTarget.transform.position - 
                gameObject.transform.position
            );
        } else {
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