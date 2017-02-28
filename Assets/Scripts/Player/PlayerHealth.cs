using UnityEngine;

public class PlayerHealth {
    public const int MaxHealth = 100;
    private int _health;

    public int Health {
        get {
            return this._health;
        }

        set {
            value = Mathf.Clamp(value, 0, PlayerHealth.MaxHealth);
            this._health = value;
        }
    }

    public PlayerHealth() {
        this.Health = PlayerHealth.MaxHealth;
    }
}