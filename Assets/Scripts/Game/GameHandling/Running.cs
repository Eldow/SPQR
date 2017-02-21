using UnityEngine;

public class Running : MonoBehaviour {
    public bool IsRunning = true;

	void Update () {
	    Time.timeScale = this.IsRunning ? 1f : 0f;
	}
}
