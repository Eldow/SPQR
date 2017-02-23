using UnityEngine;

public class Pivot : FollowTarget {
    protected Transform CameraObject;
    protected Transform PivotObject;
    protected Vector3 LastTargetPosition;
    protected Vector3 PivotDefaultPosition;
    protected Vector3 PivotDefaultRotation;

    protected override void Initialize() {
        base.Initialize();

        this.CameraObject = GetComponentInChildren<Camera>().transform;
        this.PivotObject = this.CameraObject.parent;

        this.PivotDefaultPosition = new Vector3(0, 3.4f, 0);
        this.PivotDefaultRotation = new Vector3(0, 0, 0);
        this.PivotObject.transform.position = this.PivotDefaultPosition;
        this.PivotObject.transform.rotation = 
            Quaternion.Euler(this.PivotDefaultRotation);
    }

    void Start() {
        this.Initialize();
    }

    protected virtual void UpdateCamera() {
        if (this.Target != null) {
            this.Follow(999, false);

            this.LastTargetPosition = this.Target.position;
        }

        if (Mathf.Abs(this.CameraObject.localPosition.x) > .5f ||
            Mathf.Abs(this.CameraObject.localPosition.y) > .5f) {
            this.CameraObject.localPosition = Vector3.Scale(
                this.CameraObject.localPosition,
                Vector3.forward
            );
        }

        this.CameraObject.localPosition =
            Vector3.Scale(this.CameraObject.localPosition, Vector3.forward);
    }

    protected override void Follow(float deltaTime, bool lockCamera) {
        Debug.LogWarning(
            this.GetType().Name + " warning: using Follow which is empty."
        );
    }
}
