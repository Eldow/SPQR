using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		InvokeRepeating("Test",0f,2f);
	}
	
	public void Test() {
		Debug.Log("caca");
		gameObject.GetComponent<PlayerController>().RobotStateMachine.SetState(new RobotAttackState());
	}

	// Update is called once per frame
	void Update () {
		
	}
}
