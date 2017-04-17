using UnityEngine;
using UnityEngine.AI;

public class AIFocus : MonoBehaviour
{
	public bool targetUnreachable;
	private TargetManager targetManager;
	private PlayerController pc;
	private NavMeshPath path;

	void Start ()
	{
		pc = gameObject.GetComponent<PlayerController> ();
		if (!pc.isAI)
			Destroy (this);

		targetManager = pc.TargetManager;
		path = new NavMeshPath ();
		//InvokeRepeating ("updateNavmesh", 2, 0.05f); // update regularly, not immeditately and 
	}

	private void updateNavmesh ()
	{

	}

	void Update ()
	{	

		if (!PhotonNetwork.isMasterClient)
			return;

		if (!pc.isAI)
			Destroy (this);

		//Takes a lot of resources ?
		if (targetManager.currentTarget != null) {
			NavMesh.CalculatePath (transform.position, targetManager.currentTarget.transform.position, NavMesh.AllAreas, path);
			targetUnreachable = false;
			if(path.corners.Length==0)
				targetUnreachable = true;
		}
		else {
			path.ClearCorners ();
			targetUnreachable = true;
		}

		if (targetManager.currentTarget != null && path.corners.Length > 1) {

			/*for (int i = 0; i < path.corners.Length - 1; i++) {
				Debug.DrawLine (path.corners [i], path.corners [i + 1], Color.green);	
				Debug.Log (i + " " + path.corners [i]);
			}*/

			Quaternion neededRotation;
			if (targetManager.currentTarget != null) {
				neededRotation = Quaternion.LookRotation (
					path.corners [1] -
					transform.position
				);
			} else {
				neededRotation = Quaternion.LookRotation (
					transform.forward,
					transform.up
				);
			}

			transform.rotation = Quaternion.Slerp (
				transform.rotation, 
				neededRotation, 
				Time.deltaTime * 5f
			);
		}
	}

}