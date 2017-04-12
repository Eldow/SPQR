using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StripesHandler : MonoBehaviour {

    private List<RectTransform> _stripes;
    private List<float> _randVelocities;
    private Vector3 _lastPosition;
    private Vector3 _targetPosition;
    private Vector3 _velocity = Vector3.zero;
	private InputManager inputs;

    // Use this for initialization
    void Start () {
		inputs = gameObject.AddComponent<InputManager> () as InputManager;
        _stripes = new List<RectTransform>();
        _randVelocities = new List<float>();
        foreach (RectTransform child in GetComponentsInChildren<RectTransform>())
        {
            _stripes.Add(child);
            _randVelocities.Add(Random.Range(-0.05f, 0.05f));
        }
    }

	// Update is called once per frame
	void FixedUpdate () {
        for (int i = 0; i < _stripes.Count ; i++)
        {
            _lastPosition = _stripes[i].localPosition;
			if (Mathf.Abs(inputs.cameraX()) > 0.1f)
            {
				_targetPosition = _stripes[i].localPosition + new Vector3(inputs.cameraX() * _randVelocities[i] * 5f, 0, 0);
                _stripes[i].localPosition = Vector3.SmoothDamp(_stripes[i].localPosition, _targetPosition, ref _velocity, _randVelocities[i] * Time.deltaTime);
            } else
            {
                _stripes[i].localPosition += new Vector3(_randVelocities[i] * 3f * Mathf.Sign(_targetPosition.x - _lastPosition.x), 0, 0);
            }
        }
    }
}
