using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public Running Running = null;

    public static GameManager Instance = null;

    public Dictionary<int, RobotStateMachine> PlayerList 
        { get; protected set; }
    public Dictionary<int, RobotStateMachine> AlivePlayerList 
        { get; protected set; }

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
        this.AlivePlayerList = new Dictionary<int, RobotStateMachine>();
        this.PlayerList = new Dictionary<int, RobotStateMachine>();
    }

    public virtual void RemovePlayerFromGame(int playerID) {
        RobotStateMachine robotStateMachine = this.AlivePlayerList[playerID];

        if (robotStateMachine != null) {
            this.AlivePlayerList.Remove(playerID);
        }

        robotStateMachine = this.PlayerList[playerID];

        if (robotStateMachine == null) return;

        this.PlayerList.Remove(playerID);
    }

    public virtual void AddPlayerToGame(PlayerController playerAvatar) {
        this.AlivePlayerList.Add(
            playerAvatar.ID,
            playerAvatar.RobotStateMachine
        );

        this.PlayerList.Add(
            playerAvatar.ID,
            playerAvatar.RobotStateMachine
        );
    }

    public virtual void UpdateDeadListToOthers(
        PlayerController playerController) {
        
        this.UpdateDeadList(playerController.ID);

        playerController.UpdateDeadToOthers();
    }

    public virtual void UpdateDeadList(int playerID) {
        RobotStateMachine robotStateMachine =
            this.AlivePlayerList[playerID];

        if (robotStateMachine == null) return;

        robotStateMachine.SetState(new RobotDefeatState());
        this.AlivePlayerList.Remove(playerID);

        if (!this.IsGameOver()) return;

        robotStateMachine = this.AlivePlayerList.First().Value;

        if (robotStateMachine == null) return;

        robotStateMachine.SetState(new RobotVictoryState());
    }
    protected virtual bool IsGameOver() {
        return this.AlivePlayerList.Count <= 1;
    }
}
