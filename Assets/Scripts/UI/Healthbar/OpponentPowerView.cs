using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentPowerView : MonoBehaviour {
    public float AnimationSpeed = 5f;

    private PlayerController _target;
    private float _startPosition = 0f;
    private float _position;
	private bool calledOnce = false;
    private RectTransform _rect;

	void OnEnable()
	{
		if (!calledOnce) {
			this._rect = GetComponent<RectTransform>();
			this._startPosition = this._rect.anchoredPosition.y;
			calledOnce = true;
		}
		this.UpdateTarget();
	}
		
    void Update() {

        if (this._target == null) return;

        this._position = this._startPosition*(1.0f -
            this._target.PlayerPower.Power/(float) PlayerPower.MaxPower);

        this._rect.anchoredPosition = Vector3.Lerp(
            this._rect.anchoredPosition,
            new Vector3(
                this._rect.anchoredPosition.x,
                this._position
            ),
            Time.deltaTime*this.AnimationSpeed);
    }

    protected virtual void UpdateTarget() {

		GameObject opponent = TargetManager.instance.currentTarget;

        if (opponent == null) {
            this._target = null;

            return;
        }

        this._target = opponent.GetComponent<PlayerController>();
    }
}