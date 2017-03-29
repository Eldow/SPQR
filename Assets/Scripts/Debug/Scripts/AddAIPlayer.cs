using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddAIPlayer : MonoBehaviour
{

	GameObject AIPrefab;

	void Start ()
	{
		AIPrefab = GameObject.Find ("GameManager").GetComponent<NetworkGameManager> ().AIPrefab;
	}

	public void instantiateAI ()
	{
		GameObject temp = PhotonNetwork.Instantiate (
			                  AIPrefab.name, 
			                  Vector3.left * (PhotonNetwork.room.PlayerCount * 2), 
			                  Quaternion.identity, 0
		                  );
		temp.GetComponent<PlayerPhysics> ().freezeMovement = true;
		temp.transform.name = "AIRobot";

	}
}