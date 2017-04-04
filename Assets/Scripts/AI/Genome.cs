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
	private float min,max;

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
		min = 0;
		max = 25;
		for(r=0 ; r<4 ; r++){
			if (recordTable[r] < min){
				min = recordTable[r];
			}
			if(recordTable[r] > max){
				max = recordTable[r];
			}
		}
		borderLow = min;
		borderUp = max;
	}
	
	public float GetClosest(float f) {
		if(recordTable == null){
			recordTable = new float[5];
			for(r=0 ; r<5 ; r++){recordTable[r] = 0;}
		}
		min = 25f;
		for(r = 0;r<4;r++){
			if(Mathf.Abs(f-recordTable[r]) < min){
				min = Mathf.Abs(f-recordTable[r]);
			}
		}
		return min;
	}
}

public class Genome {

	public Gene[] dna;
	
	public Genome() {
		dna = new Gene[4];
		//idle
		dna[0].SetBorderLow(0);
		dna[0].SetBorderUp(25);
		//walk
		dna[1].SetBorderLow(0);
		dna[1].SetBorderUp(25);
		//attack
		dna[2].SetBorderLow(0);
		dna[2].SetBorderUp(4);
		//block
		dna[3].SetBorderLow(0);
		dna[3].SetBorderUp(8);
	}
}