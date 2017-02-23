using UnityEngine;

public class DebugViewPoint : MonoBehaviour {
    public float Radius = .1f;
    public Color Color = Color.yellow;

    [HideInInspector]
    public Vector3 Position = Vector3.zero;

    void Start() {
        this.UpdatePositions();
    }

    void OnDrawGizmos() {
        this.UpdatePositions();
        Gizmos.color = this.Color;
        Gizmos.DrawSphere(this.Position, this.Radius);
    }

    void UpdatePositions() {
        this.Position = this.gameObject.transform.position;
    }
}
