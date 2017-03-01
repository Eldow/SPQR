using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public Running Running = null;

    public static GameManager Instance = null;

    public PlayerController MasterPlayer;

    public Dictionary<int, PlayerController> PlayersList;
    public Dictionary<int, PlayerController> AlivePlayersList;

    void Awake() {
        if (GameManager.Instance == null) {
            GameManager.Instance = this;
            this.Initialize();
        } else if (GameManager.Instance != this) {
            Destroy(gameObject);
        }

        GameObject.DontDestroyOnLoad(gameObject);

        if (this.Running != null) return;

        this.Running = this.gameObject.GetComponent<Running>();

        if (this.Running == null) {
            Debug.LogError(this.GetType().Name + ": No Running script found!");
        }
    }

    void Update() {
        
    }

    protected virtual void Initialize() {
        this.AlivePlayersList = new Dictionary<int, PlayerController>();
        this.PlayersList = new Dictionary<int, PlayerController>();
    }

    public virtual void RemovePlayerToGame(int playerID) {
        PlayerController robotStateMachine = this.AlivePlayersList[playerID];

        if (robotStateMachine != null) {
            this.AlivePlayersList.Remove(playerID);
        }

        robotStateMachine = this.PlayersList[playerID];

        if (robotStateMachine == null) return;

        this.PlayersList.Remove(playerID);
    }

    public virtual void AddPlayerToGame(PlayerController playerAvatar) {
        this.AlivePlayersList.Add(
            playerAvatar.ID,
            playerAvatar
        );

        this.PlayersList.Add(
            playerAvatar.ID,
            playerAvatar
        );
    }

    public virtual void CheckIfGameOver() {
        if (!this.IsGameOver()) return;
        
        MasterPlayer.SendStateToOthers(
            this.AlivePlayersList.First().Key, 
            "RobotVictoryState");
    }

    public virtual void UpdateDeadList(int playerID) {
        PlayerController robotStateMachine =
            this.AlivePlayersList[playerID];

        if (robotStateMachine == null) return;

        this.AlivePlayersList.Remove(playerID);

        if (!this.IsGameOver()) return;

        robotStateMachine = this.AlivePlayersList.First().Value;

        if (robotStateMachine == null) return;

    }

    public virtual void UpdatePlayerHealth(int playerId, int health)
    {
        this.PlayersList[playerId].PlayerHealth.Health = health;

        if (!PhotonNetwork.player.IsMasterClient) return;

        if (health > 0) return;

        if (!this.AlivePlayersList.Remove(playerId)) return;

        this.MasterPlayer.SendStateToOthers(playerId, "RobotDefeatState");
    }

    protected virtual bool IsGameOver() {
        return this.AlivePlayersList.Count <= 1;
    }
}
