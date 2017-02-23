using UnityEngine;

public class DebugViewVector : MonoBehaviour {
    public GameObject StartObject = null;
    public GameObject EndObject = null;
    public Color Color = Color.red;

    [HideInInspector]
    public Vector3 StartPosition = Vector3.zero;
    [HideInInspector]
    public Vector3 EndPosition = Vector3.up;

    void Start() {
        if (this.StartObject == null || this.EndObject == null) {
            Debug.LogError(this.GetType().Name + " error: Wrong parameters!");
        }

        this.UpdatePositions();
    }

    void OnDrawGizmos() {
        this.UpdatePositions();
        Gizmos.color = this.Color;
        Gizmos.DrawLine(this.StartPosition, this.EndPosition);
    }

    void UpdatePositions() {
        this.StartPosition = this.StartObject.transform.position;
        this.EndPosition = this.EndObject.transform.position;
    }
}
