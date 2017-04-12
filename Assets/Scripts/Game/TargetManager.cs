using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

    This class manages the references on the player and its opponents

*/

public class TargetManager : MonoBehaviour {

    // Player references
    public List<GameObject> opponents;
	public GameObject currentTarget = null;
	public string ownTeam;


	private void updateOpponents()
	{
		ownTeam = gameObject.GetComponent<PlayerController> ().Team;

		foreach(KeyValuePair<int,RobotStateMachine> pair in GameManager.Instance.AlivePlayerList)
		{
			if (pair.Value.PlayerController.Team != ownTeam) {
				opponents.Add (pair.Value.gameObject);
			}
		}

	}

    // TODO : Multiple ennemy focus - Gets nearest opponent to focus
	public GameObject updateNearestOpponent()
	{
		updateOpponents ();

		if (opponents.Count > 0) {
			
			float minDistance = Mathf.Infinity;
			int minIndex = 0;
			for (int i = 0; i < opponents.Count; i++) {
				float currentDist = Vector3.Distance (opponents [i].transform.position, transform.position);
				if (currentDist < minDistance) {
					minDistance = currentDist;
					minIndex = i;
				}
			}
			currentTarget = opponents [minIndex];
		} else {
			currentTarget = null;
		}

		return currentTarget;
	}
}
