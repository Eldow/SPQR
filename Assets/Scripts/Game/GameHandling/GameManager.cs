using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public Running Running = null;

    public static GameManager Instance = null;

    protected Dictionary<int, RobotStateMachine> PlayersList;
    protected Dictionary<int, RobotStateMachine> AlivePlayersList;

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
        this.AlivePlayersList = new Dictionary<int, RobotStateMachine>();
        this.PlayersList = new Dictionary<int, RobotStateMachine>();
    }

    public virtual void RemovePlayerToGame(int playerID) {
        RobotStateMachine robotStateMachine = this.AlivePlayersList[playerID];

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
            playerAvatar.RobotStateMachine
        );

        this.PlayersList.Add(
            playerAvatar.ID,
            playerAvatar.RobotStateMachine
        );
    }

    public virtual void UpdateDeadList(int playerID) {
        RobotStateMachine robotStateMachine = this.AlivePlayersList[playerID];

        if (robotStateMachine == null) return;

        robotStateMachine.SetState(new RobotDefeatState());
        this.AlivePlayersList.Remove(playerID);

        if (!this.IsGameOver()) return;

        robotStateMachine =  this.AlivePlayersList.First().Value;

        if (robotStateMachine == null) return;

        robotStateMachine.SetState(new RobotVictoryState());
    }

    protected virtual bool IsGameOver() {
        return this.AlivePlayersList.Count <= 1;
    }
}
