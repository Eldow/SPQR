using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentPowerView : MonoBehaviour {
    public float AnimationSpeed = 5f;

    private PlayerController _target;
    private float _startPosition = 0f;
    private float _position;
    private RectTransform _rect;

    void Start() {
        this._rect = GetComponent<RectTransform>();
        this._target
            = TargetManager.instance.player.GetComponent<PlayerController>();
        this._startPosition = this._rect.anchoredPosition.y;
        this.UpdateTarget();
    }

    void Update() {
        this.UpdateTarget();

        this._position = this._startPosition * (1.0f -
            this._target.PlayerPower.Power / (float)PlayerPower.MaxPower);
        this._rect.anchoredPosition = Vector3.Lerp(
            this._rect.anchoredPosition,
            new Vector3(
                this._rect.anchoredPosition.x,
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
