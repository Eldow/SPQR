using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public Running Running = null;
	public bool isLocalGame = true;
    public static GameManager Instance = null;

    public PlayerController LocalPlayer = null;

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
        RobotStateMachine robotStateMachine = null;

        try {
            robotStateMachine = this.AlivePlayerList[playerID];
        } catch (KeyNotFoundException exception) {
            Debug.LogWarning(
                "RemovePlayerFromGame: key " + playerID + " was not found");
            Debug.LogWarning(exception.Message);
        }

        if (robotStateMachine != null) {
            this.AlivePlayerList.Remove(playerID);
        }

        robotStateMachine = this.PlayerList[playerID];

        if (robotStateMachine == null) return;

        this.PlayerList.Remove(playerID);
    }

    public virtual void AddPlayerToGame(PlayerController playerAvatar) {
		if (!playerAvatar.photonView.isMine)
			isLocalGame = false;

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

        RobotStateMachine robotStateMachine =
             playerController.RobotStateMachine;

        if (robotStateMachine == null) return;

        robotStateMachine.SetState(new RobotDefeatState());
    }

    public virtual void UpdateDeadList(int playerID) {
        try {
            RobotStateMachine robotStateMachine = null;

            robotStateMachine = this.AlivePlayerList[playerID];

            if (robotStateMachine == null) return;

            this.AlivePlayerList.Remove(playerID);

            if (!this.IsGameOver()) return;

            if (this.AlivePlayerList.Count <= 0) return;

            robotStateMachine = this.AlivePlayerList.First().Value;

            if (robotStateMachine == null) return;

            robotStateMachine.SetState(new RobotVictoryState());
        } catch (KeyNotFoundException exception) {
            Debug.LogWarning(
                "UpdateDeadList: key " + playerID + " was not found");
            Debug.LogWarning(exception.Message);
        }
    }
    protected virtual bool IsGameOver() {

		string teamFound = null;
		foreach(KeyValuePair<int,RobotStateMachine> pair in GameManager.Instance.AlivePlayerList)
		{
			if (pair.Value.PlayerController.Team != teamFound) {
				if (teamFound == null)
					teamFound = pair.Value.PlayerController.Team;
				else
					return false; // there are at least 2 different teams;
			}
		}
		return true;
    }
}
