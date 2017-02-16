using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		MovieTexture movie = GetComponent<Renderer>().material.mainTexture as MovieTexture;
		movie.loop = true;
		movie.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
