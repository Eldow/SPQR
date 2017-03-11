using UnityEngine;

public class OpponentHealthView : MonoBehaviour {
    public float AnimationSpeed = 5f;

    private PlayerController _target;
    private float _startPosition = 0f;
    private float _position;
	private bool calledOnce = false;
    private RectTransform _rect;

	void OnEnable()
	{
		if (!calledOnce) {
			this._rect = GetComponent<RectTransform> ();
			this._startPosition = this._rect.anchoredPosition.x;
			calledOnce = true;
		}
		this.UpdateTarget();
	}

    void Update() {

        if (this._target == null) return;

        this._position = this._startPosition * (1.0f -
            this._target.PlayerHealth.Health / (float)PlayerHealth.MaxHealth);

        this._rect.anchoredPosition = Vector3.Lerp(
            this._rect.anchoredPosition,
            new Vector3(
                this._position,
                this._rect.anchoredPosition.y
            ),
            Time.deltaTime * this.AnimationSpeed);
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
