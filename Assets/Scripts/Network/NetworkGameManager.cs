using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkGameManager : Photon.PunBehaviour {
	public GameObject AIPrefab;
    public GameObject PlayerPrefab;
    protected GameObject PlayerAvatar;
    protected PhotonView PhotonView;

    void Start() {
        if (!PhotonNetwork.connected) return;

        GameObject localPlayer = PhotonNetwork.Instantiate(
            PlayerPrefab.name, 
            Vector3.left * (PhotonNetwork.room.PlayerCount * 2), 
            Quaternion.identity, 0
        );

        GameManager.Instance.LocalPlayer 
            = localPlayer.GetComponent<PlayerController>();
    }

    public override void OnLeftRoom() {
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
    }

    void LoadArena() {
        if (!PhotonNetwork.isMasterClient) return;

        PhotonNetwork.LoadLevel("Sandbox");
    }
}
