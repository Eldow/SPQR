using UnityEngine;

public class PlayerPower {
    public const int MaxPower = 100;
    private int _power;

    public int Power {
        get {
            return this._power;
        }

        set {
            value = Mathf.Clamp(value, 0, PlayerPower.MaxPower);
            this._power = value;
        }
    }

    public PlayerPower() {
        this.Power = PlayerPower.MaxPower;
    }
}