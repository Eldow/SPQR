using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

    This class manages the references on the player and its opponents

*/

public class TargetManager : MonoBehaviour {

    // Player references
    public List<GameObject> opponents;
	public GameObject player;
	public GameObject currentTarget = null;

    // Public static intance to the manager
    public static TargetManager instance = null;

    // Stores a static instance of this target manager to access it from anywhere at anytime
    void Awake ()
    {
        if(instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(this);
    }

    // Initializes members
    public void Start()
    {
        opponents = new List<GameObject>();
    }

    // Adds an opponent to the array Opponents
    public void AddOpponent(GameObject opponent)
    {
        opponents.Add(opponent);
    }

    // Sets the local player
    public void SetPlayer(GameObject localPlayer)
    {
        player = localPlayer;
    }
    
    // TODO : Multiple ennemy focus - Gets nearest opponent to focus
	public void updateNearestOpponent()
	{
		if (opponents.Count > 0) {
			
			float minDistance = Mathf.Infinity;
			int minIndex = 0;
			for (int i = 0; i < opponents.Count; i++) {
				float currentDist = Vector3.Distance (opponents [i].transform.position, player.transform.position);
				if (currentDist < minDistance) {
					minDistance = currentDist;
					minIndex = i;
				}
			}
			currentTarget = opponents [minIndex];
		} else {
			currentTarget = null;
		}
	}
}
