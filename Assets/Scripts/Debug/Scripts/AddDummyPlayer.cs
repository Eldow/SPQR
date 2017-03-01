using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddDummyPlayer : MonoBehaviour
{

	GameObject PlayerPrefab;

	void Start ()
	{
		PlayerPrefab = GameObject.Find ("GameManager").GetComponent<NetworkGameManager> ().PlayerPrefab;
	}

	public void instantiateDummy ()
	{
		GameObject temp = PhotonNetwork.Instantiate (
			                  PlayerPrefab.name, 
			                  Vector3.left * (PhotonNetwork.room.PlayerCount * 2), 
			                  Quaternion.identity, 0
		                  );
		temp.GetComponent<PlayerController> ().isDummy = true;
		temp.GetComponent<PlayerPhysics> ().freezeMovement = true;
		temp.transform.name = "DummyRobot";

	}
}
