using UnityEngine;

public class PlayerHealthView : MonoBehaviour {
    public float AnimationSpeed = 5f;

    private PlayerController _target;
    private float _startRotation = 136f;
    private float _rotation;
    private RectTransform _rect;

    void Start() {
        this._rect = GetComponent<RectTransform>();
        this._target = 
            TargetManager.instance.player.GetComponent<PlayerController>();
        this._startRotation = this._rect.eulerAngles.z;
        this._rect.eulerAngles = 
            new Vector3(this._rect.eulerAngles.x, this._rect.eulerAngles.y, 0);
    }

    void FixedUpdate() {
        this._rotation = this._startRotation * (1.0f -
            this._target.PlayerHealth.Health / (float)PlayerHealth.MaxHealth);

        this._rect.eulerAngles = Vector3.Lerp(
            this._rect.eulerAngles, 
            new Vector3(
                this._rect.eulerAngles.x, 
                this._rect.eulerAngles.y, 
                this._rotation
            ), 
            Time.deltaTime * this.AnimationSpeed
        );
    }
}
