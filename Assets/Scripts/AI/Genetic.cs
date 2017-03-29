using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Gene 
{
	//floats settings link to state parameter
	private float borderLow, borderUp;
	//floats table to record precedent succesful actions
	private float[] recordTable;
	//local recorder
	private int r;

	//Getters & setters
	public float GetBorderLow() {return borderLow;}
	public float GetBorderUp() {return borderUp;}
	public void SetBorderLow(float set_) {borderLow = set_;}
	public void SetBorderUp(float set_) {borderUp = set_;}
	public float GetRecordTable(int pos) {return recordTable[pos];}
	public void SetRecordTable(float set_) {
		if(recordTable == null){
			recordTable = new float[5];
			for(r=0 ; r<5 ; r++){recordTable[r] = 0;}
		}
		else{
			recordTable[(int) recordTable[4]] = set_;
			recordTable[4] = (float) ( ((int)(recordTable[4]+1f))%5 );
		}
	}
}

public class Genetic : MonoBehaviour {

	private Gene[] dna;
	
	public void Start() {
		dna = new Gene[4];
		//idle
		dna[0].SetBorderLow(0);
		dna[0].SetBorderUp(25);
		//walk
		dna[0].SetBorderLow(0);
		dna[0].SetBorderUp(25);
		//attack
		dna[0].SetBorderLow(0);
		dna[0].SetBorderUp(25);
		//block
		dna[0].SetBorderLow(0);
		dna[0].SetBorderUp(25);
	}
}