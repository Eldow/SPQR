using UnityEngine;
using UnityEngine.UI;

public class DebugViewer : MonoBehaviour {
    [HideInInspector]
    public Text TextObject = null;

    // to be overriden in child, if necessary
    public virtual string Label {
        get {
            return "";
        }
    }

    void Awake() {
        this.TextObject = this.gameObject.GetComponent<Text>();
    }

    void Start () {
		
	}

	void Update () {
		
	}
}
