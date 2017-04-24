using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectateCamera : MonoBehaviour {

    public float SpeedX = 2f;
    public float SpeedY = 2f;
    public float Width = 15f;
    public float Height = 15f;
    public float YPosition = 10f;

    private float _alphaX = 0f;
    private float _alphaY = 0f;

    public static Camera SpectatorCamera;

    // Use this for initialization
    void Start () {
        SpectatorCamera = GetComponent<Camera>();
        SpectatorCamera.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        _alphaX += SpeedX;
        _alphaY += SpeedY;
        float X = (Width * Mathf.Cos(_alphaX * .005f));
        float Y = (Height * Mathf.Sin(_alphaY * .005f));
        this.gameObject.transform.position = new Vector3(X, YPosition, Y);
        transform.LookAt(Vector3.zero);
    }
}
