using UnityEngine;

public class PlayerHealth {
    public const int MaxHealth = 100;

    public PlayerController PlayerController { get; protected set; }

    private int _health = PlayerHealth.MaxHealth;

    public int Health {
        get {
            return this._health;
        }

        set {
            /* Prevent GameManager.Instance.UpdateDeadListToOthers from being
             * called several times. */
            if (this._health == 0) return;

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