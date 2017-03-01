using UnityEngine;

public class PlayerHealth {
    public const int MaxHealth = 100;

    public PlayerController PlayerController { get; protected set; }

    private int _health;

    public int Health {
        get {
            return this._health;
        }

        set {
            value = Mathf.Clamp(value, 0, PlayerHealth.MaxHealth);
            this._health = value;

            if (this._health > 0) return;

            if (!this.PlayerController.photonView.isMine) return;

            GameManager.Instance.UpdateDeadListToOthers(this.PlayerController);
        }
    }

    public PlayerHealth(PlayerController playerController) {
        this.Health = PlayerHealth.MaxHealth;
        this.PlayerController = playerController;
    }
}