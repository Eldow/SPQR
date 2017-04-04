using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StripesHandler : MonoBehaviour {

    private List<RectTransform> _stripes;
    private List<float> _randVelocities;

	// Use this for initialization
	void Start () {
        _stripes = new List<RectTransform>();
        _randVelocities = new List<float>();
        foreach (RectTransform child in GetComponentsInChildren<RectTransform>())
        {
            _stripes.Add(child);
            _randVelocities.Add(Random.Range(-.15f, .15f));
        }
    }
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < _stripes.Count ; i++)
        {
            if(i%2==0)
                _stripes[i].localPosition += new Vector3(_randVelocities[i], 0);
            else
                _stripes[i].localPosition -= new Vector3(_randVelocities[i], 0);
        }
    }
}
