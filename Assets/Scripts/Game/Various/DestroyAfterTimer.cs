using UnityEngine;

public class DestroyAfterTimer : MonoBehaviour {
    public float TimeToLive = 30f;

	void Update () {
        this.TimeToLive -= Time.deltaTime;

	    if (this.TimeToLive > 0) return;

	    GameObject.Destroy(this.gameObject);
	}
}
