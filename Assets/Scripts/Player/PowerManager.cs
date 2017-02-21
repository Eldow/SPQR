using UnityEngine;

public class PowerManager : Photon.MonoBehaviour {
    public const int MaxPower = 100;
    public int CurrentPower = 0;
    public int OtherPower = 0;

    void Awake() {
        this.CurrentPower = MaxPower;
        this.OtherPower = MaxPower;
    }

    void Update() {
        if (!photonView.isMine) {
            OtherPower = this.CurrentPower;
        }

        if (photonView.isMine) {
            this.CurrentPower = OtherPower;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream,
        PhotonMessageInfo info) {
        if (stream.isWriting) {
            stream.SendNext(this.CurrentPower);
        } else if (stream.isReading) {
            this.CurrentPower = (int)stream.ReceiveNext();
        }
    }

    public void IncreasePower(int amount) {
        if (!photonView.isMine) {
            return;
        }

        CurrentPower += amount;

        if (CurrentPower >= MaxPower) {
            // TODO : Set overheat state
            Debug.Log("Overheat!");
        }

        Debug.Log(CurrentPower);
    }
}