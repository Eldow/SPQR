using UnityEngine;

public class HealthManager : Photon.MonoBehaviour {
    public const int MaxHealth = 100;
    public int CurrentHealth = 0;
    public int OtherHealth = 0;

    void Awake() {
        this.CurrentHealth = MaxHealth;
        this.OtherHealth = MaxHealth;
    }

    void Update() {
        if (!photonView.isMine) {
            OtherHealth = this.CurrentHealth;
        }

        if (photonView.isMine) {
            this.CurrentHealth = OtherHealth;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, 
        PhotonMessageInfo info) {
        if (stream.isWriting) {
            stream.SendNext(this.CurrentHealth);
        } else if (stream.isReading) {
            this.CurrentHealth = (int)stream.ReceiveNext();
        }
    }

    public void TakeDamage(int amount) {
        if (!photonView.isMine) {
            return;
        }

        CurrentHealth -= amount;

        if (CurrentHealth <= 0) {
            // TODO : Set dead state
            CurrentHealth = 0;
            Debug.Log("Dead!");
            // set state machine
        }

        Debug.Log(CurrentHealth);
    }
}