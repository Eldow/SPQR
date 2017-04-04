using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour {

	private float distanceToOpponent;
	private GameObject player;
	private int health;
	private int ennemyHealth_;
	private Genome genome;
	private RobotStateMachine stateMachine;
	private PlayerHealth robotHealth;
	private PlayerHealth ennemyHealth;
	private InputManager inputManager;
	
	//local registers
	private int r, r1,i, frameAttackReg, frameAttackThreshold;
	private float f,f1,rand;

	void FindPlayer() {
		if (TargetManager.instance == null) return;
        player = TargetManager.instance.player;
	}
	
	// Use this for initialization
	void Start () {
		FindPlayer();
		genome = new Genome();
		stateMachine = gameObject.GetComponent<PlayerController>().RobotStateMachine;
		robotHealth = gameObject.GetComponent<PlayerController>().PlayerHealth;
		ennemyHealth = player.GetComponent<PlayerController>().PlayerHealth;
		health = robotHealth.Health;
		r = health;
		ennemyHealth_ = ennemyHealth.Health;
		r1 = ennemyHealth_;
		frameAttackThreshold = 60;
		frameAttackReg = 0;
		inputManager = ((RobotStateMachine) stateMachine).PlayerController.inputManager;
	}
	
	void Learn (bool b) {
		rand = Random.Range(0f,1f);
		if(b){
			if(r<0.75f){
				genome.dna[3].SetRecordTable(distanceToOpponent);
			}
			else{
				genome.dna[2].SetRecordTable(distanceToOpponent);
			}
		}
		else{
			if(r<0.75f){
				genome.dna[2].SetRecordTable(distanceToOpponent);
			}
			else{
				genome.dna[3].SetRecordTable(distanceToOpponent);
			}
		}
	}
	
	//might be more interesting if unique for each state
	float SetActionForce (int action,float a) {
		f1 = genome.dna[action].GetClosest(a);
		return ( 1f - (f/(genome.dna[action].GetBorderUp()-genome.dna[action].GetBorderLow())) );
	}
	
	void StopButtonAttack(){
		inputManager.attackButtonAI = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (player != null) {
            //updating environment
			r = robotHealth.Health;
			r1 = ennemyHealth.Health;
			if(health != r){
				Learn(true);
				health = r;
			}
			if(ennemyHealth_ != r1){
				Learn(false);
				ennemyHealth_ = r1;
			}
			distanceToOpponent = Vector3.Distance(gameObject.transform.position,player.transform.position);
			//chose action
			for(i=0 ; i<1 ;i++){
				//1st action priority : attack
				if(distanceToOpponent > genome.dna[2].GetBorderLow() && distanceToOpponent < genome.dna[2].GetBorderUp()){
					f = SetActionForce(2,distanceToOpponent);
					//rand = Random.Range(0f,1f);
					if (f > 0.9f){
						if(frameAttackReg == frameAttackThreshold){
							Debug.Log("Attack");
							frameAttackReg = 0;
							inputManager.attackButtonAI = true;
							Invoke("StopButtonAttack",0.1f);
							break;
						}
					}
					else{
						//2nd action priority : block
						if(distanceToOpponent > genome.dna[3].GetBorderLow() && distanceToOpponent < genome.dna[3].GetBorderUp()){
							f = SetActionForce(3,distanceToOpponent);
							//rand = Random.Range(0f,1f);
							if (f > 0.9f){
								if(frameAttackReg == frameAttackThreshold){
									if(!inputManager.blockButtonAI){
										Debug.Log("block");
										inputManager.blockButtonAI = true;
										break;
									}
								}
							}
							else{
								if(inputManager.blockButtonAI){
									Debug.Log("stopBlock");
									inputManager.blockButtonAI = false;
								}
							}
						}
					}
				}
				//3rd action priority : walk
				if(distanceToOpponent > genome.dna[1].GetBorderLow() && distanceToOpponent < genome.dna[1].GetBorderUp()){
					f = SetActionForce(1,distanceToOpponent);
					rand = Random.Range(0f,1f);
					if (rand < f){
						Debug.Log("walk");
						inputManager.moveY();
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
		else {
			FindPlayer();
        }
		if(frameAttackReg == frameAttackThreshold){frameAttackReg = 0;}
		frameAttackReg++;
	}
}
