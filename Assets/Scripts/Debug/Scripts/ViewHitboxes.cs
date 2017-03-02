using UnityEngine;

[ExecuteInEditMode]
public class ViewHitboxes : MonoBehaviour {
    public Color Color = Color.yellow;
    public string GameObjectTag = PlayerController.Player;

    protected GameObject GameObject;

    void Start() {
        this.TryToFindGameObject();
    }

    void OnDrawGizmos() {
        if (this.GameObject == null || !this.GameObject.activeSelf) {
            this.TryToFindGameObject();
        }

        if (this.GameObject == null) return;

        Transform parent = this.GameObject.transform;

        if (parent == null) return;

        Gizmos.color = this.Color;

        this.FindHitboxes(parent);
    }

    protected virtual void TryToFindGameObject() {
        this.GameObject = GameObject.FindGameObjectWithTag(this.GameObjectTag);
    }

    protected virtual void FindHitboxes(Transform parent) {
        this.FindHitboxes(parent, parent.localScale);
    }

    protected virtual void FindHitboxes(Transform parent, 
        Vector3 parentScale) {
        Vector3 childScale;

        foreach (Transform child in parent) {
            childScale = new Vector3(
                parentScale.x * child.localScale.x,
                parentScale.y * child.localScale.y,
                parentScale.z * child.localScale.z);

            this.FindHitboxes(child, childScale);
            Collider collider = null;

            if ((collider = child.GetComponent<Collider>()) == null) {
                continue;
            }

            this.DrawHitbox(collider, childScale);
        }
    }

    protected virtual void DrawHitbox(Collider collider, Vector3 scale) {
        if (collider is SphereCollider) {
            this.DrawSphere((SphereCollider) collider, scale);
        } else if (collider is BoxCollider) {
            this.DrawBox((BoxCollider)collider, scale);
        }
    }

    protected virtual void DrawSphere(SphereCollider sphereCollider, 
        Vector3 scale) {
        Gizmos.DrawSphere(
            sphereCollider.transform.TransformPoint(sphereCollider.center), 
            sphereCollider.radius * Mathf.Max(
                scale.x, Mathf.Max(scale.y, scale.z)));
    }

    protected virtual void DrawBox(BoxCollider boxCollider, Vector3 scale) {
        Gizmos.DrawCube(
            boxCollider.transform.TransformPoint(boxCollider.center), 
            boxCollider.size * scale.x);
    }
}