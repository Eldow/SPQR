using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour {

	private float distanceToOpponent;
	private GameObject player;
	private int health;
	private Genome genome;
	private RobotStateMachine stateMachine;
	private PlayerHealth robotHealth;
	private InputManager inputManager;
	
	//local registers
	private int r,i,frameAttack,frameAttackReg;
	private float f,rand;

	void FindPlayer() {
		if (TargetManager.instance == null) return;
        player = TargetManager.instance.player;
	}
	
	// Use this for initialization
	void Start () {
		FindPlayer();
		health = gameObject.GetComponent<PlayerController>().PlayerHealth.Health;
		genome = new Genome();
		stateMachine = gameObject.GetComponent<PlayerController>().RobotStateMachine;
		robotHealth = gameObject.GetComponent<PlayerController>().PlayerHealth;
		frameAttack = 30;
		frameAttackReg = 0;
		inputManager = ((RobotStateMachine) stateMachine).PlayerController.inputManager;
	}
	
	void Learn () {
		
	}
	
	//might be more interesting if unique for each state
	float SetActionForce (float a) {
		return ( (a)/(25f));
	}
	
	// Update is called once per frame
	void Update () {
		if (player != null) {
            //updating environment
			distanceToOpponent = Vector3.Distance(gameObject.transform.position,player.transform.position);
			r = robotHealth.Health;
			if(health != r){
				Learn();
				health = r;
			}
			//chose action
			for(i=0 ; i<1 ;i++){
				//1st action priority : attack
				if(distanceToOpponent > genome.dna[2].GetBorderLow() && distanceToOpponent < genome.dna[2].GetBorderUp()){
					f = SetActionForce(distanceToOpponent);
					rand = Random.Range(0f,1f);
					if (rand > f){
						if(frameAttackReg == 60){
							Debug.Log("Attack");
							frameAttackReg = 0;
							inputManager.attackButton();
							break;
						}
					}
					else{
						//2nd action priority : block
						if(distanceToOpponent > genome.dna[3].GetBorderLow() && distanceToOpponent < genome.dna[3].GetBorderUp()){
							
							f = SetActionForce(distanceToOpponent);
							rand = Random.Range(0f,1f);
							if (rand > f){
								if(frameAttackReg == 60){
									Debug.Log("block");
									stateMachine.SetState(new RobotBlockState());
									break;
								}
							}
							else{
								//3rd action priority : walk
								if(distanceToOpponent > genome.dna[1].GetBorderLow() && distanceToOpponent < genome.dna[1].GetBorderUp()){
									f = SetActionForce(distanceToOpponent);
									rand = Random.Range(0f,1f);
									if (rand < f){
										Debug.Log("walk");
										stateMachine.SetState(new RobotWalkState());
										break;
									}
									else{
										// 4th action priority : idle
										// if(distanceToOpponent > genome.dna[0].GetBorderLow() && distanceToOpponent < genome.dna[0].GetBorderUp()){
											// f = SetActionForce(distanceToOpponent);
											// rand = Random.Range(0f,1f);
											// if (rand < f){
												// gameObject.GetComponent<PlayerController>().RobotStateMachine.SetState(new RobotIdleState());
											// }
										// }
									}
								}
							}
						}
					}
				}
			}
        }
		else {
			FindPlayer();
        }
		if(frameAttackReg == 60){frameAttackReg = 0;}
		frameAttackReg++;
	}
}
