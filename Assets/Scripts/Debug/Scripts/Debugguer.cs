using UnityEngine;

public class Debugguer : MonoBehaviour {
    public bool IsEnabled = true;
    public GameObject DebuggerContainer = null;

	void Update () {
	    if (this.DebuggerContainer == null) return;

	    this.DebuggerContainer.SetActive(this.IsEnabled);
	}
}
