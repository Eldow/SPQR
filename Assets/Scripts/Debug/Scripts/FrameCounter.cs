using UnityEngine;

public class FrameCounter : MonoBehaviour {
    [HideInInspector]
    public int FramePerSecond = 1;
    public int CurrentFrame { get; protected set; }

    void Awake() {
        this.FramePerSecond = (int)(1 / Time.fixedDeltaTime);
        this.CurrentFrame = 0;
    }

    void FixedUpdate () {
        this.CurrentFrame = (this.CurrentFrame + 1) % this.FramePerSecond;
    }
}
