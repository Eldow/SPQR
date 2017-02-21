using UnityEngine;
using UnityEngine.UI;

public class ViewCurrentFrame : MonoBehaviour {
    public FrameCounter FrameCounter = null;

    [HideInInspector]
    public Text FrameCounterText = null;
    public const string Label = "Frame: ";

    void Awake() {
        this.FrameCounterText = this.gameObject.GetComponent<Text>();
    }

	void Update () {
	    if (this.FrameCounter == null || this.FrameCounterText == null) return;

	    this.FrameCounterText.text =
            ViewCurrentFrame.Label + this.FrameCounter.CurrentFrame;
	}
}
