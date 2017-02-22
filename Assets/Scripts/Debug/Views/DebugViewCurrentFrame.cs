using UnityEngine;
using UnityEngine.UI;

public class DebugViewCurrentFrame : DebugViewer {
    public FrameCounter FrameCounter = null;

    // to be overriden in child, if necessary
    public override string Label {
        get {
            return "Frame: ";
        }
    }

	void Update () {
	    if (this.FrameCounter == null || this.TextObject == null) return;

	    this.TextObject.text =
            this.Label + this.FrameCounter.CurrentFrame;
	}
}
