using UnityEngine;

public class OpponentHealthView : MonoBehaviour {
    public float AnimationSpeed = 5f;

    private PlayerController _target;
    private float _startPosition = 0f;
    private float _position;
    private RectTransform _rect;

    void Start() {
        this._rect = GetComponent<RectTransform>();
        this._startPosition = this._rect.anchoredPosition.y;
        this.UpdateTarget();
    }

    void Update() {
        this.UpdateTarget();

        this._position = this._startPosition * (1.0f -
            this._target.PlayerHealth.Health / (float)PlayerHealth.MaxHealth);
        this._rect.anchoredPosition = Vector3.Lerp(
            this._rect.anchoredPosition,
            new Vector3(
                this._rect.anchoredPosition.y,
                this._position
            ),
            Time.deltaTime * this.AnimationSpeed);
    }

    protected virtual void UpdateTarget() {
        GameObject opponent
            = TargetManager.instance.GetNearestOpponent();

        if (opponent == null) {
            this._target = null;

            return;
        }

        this._target = opponent.GetComponent<PlayerController>();
    }
}
