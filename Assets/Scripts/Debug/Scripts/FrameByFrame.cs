using UnityEngine;

public class FrameByFrame : MonoBehaviour {
    public bool Enable = false;
    private bool _hasChanged = false;

    void Update() {
        this.CheckIfEnabled();

        if (this._hasChanged) {
            // enabling it the first frame the button has been pressed
            GameManager.Instance.Running.IsRunning = !this.Enable;

            this._hasChanged = false;
        } else {
            if (!this.Enable) return;

            GameManager.Instance.Running.IsRunning = false;

            if (!InputManager.nextFrame()) return;

            GameManager.Instance.Running.IsRunning = true;
        }
    }

    protected virtual void CheckIfEnabled() {
        if (!InputManager.frameByFrame()) {
            return;
        }

        this.Enable = !this.Enable;
        this._hasChanged = true;
    }
}
