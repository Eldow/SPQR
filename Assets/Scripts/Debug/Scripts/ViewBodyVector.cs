using UnityEngine;

public class ViewBodyVector : MonoBehaviour {
    [HideInInspector]
    public PlayerPhysics PlayerPhysics;

    [HideInInspector]
    public DebugViewVector DebugViewVector = null;

	void Start () {
	    this.DebugViewVector = this.gameObject.GetComponent<DebugViewVector>();
	}
	
	void Update () {
	    if (this.PlayerPhysics == null) {
	        this.TryToGetCameraVector();
	    }

        this.DebugViewVector.StartObject.transform.position
            = this.PlayerPhysics.gameObject.transform.position;

        this.DebugViewVector.EndObject.transform.position 
            = this.PlayerPhysics.RigidBody.velocity;
	}

    protected void TryToGetCameraVector() {
        GameObject player =
            GameObject.FindGameObjectWithTag(PlayerController.Player);

        if (player == null) return;

        this.PlayerPhysics = player.GetComponent<PlayerPhysics>();
    }
}
