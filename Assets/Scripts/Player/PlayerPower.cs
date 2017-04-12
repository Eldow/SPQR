using UnityEngine;

public class PlayerPower {
    public const float MaxPower = 100;

    public PlayerController PlayerController { get; protected set; }

    private float _power = PlayerPower.MaxPower;

    public float Power {
        get {
            return this._power;
        }

        set {
            value = Mathf.Clamp(value, 0, PlayerPower.MaxPower);
            this._power = value;
        }
    }

    public PlayerPower(PlayerController playerController) {
        this.Power = PlayerPower.MaxPower;
        this.PlayerController = playerController;
    }
}