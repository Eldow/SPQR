using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public Running Running = null;
	  public bool isLocalGame = true;
    public static GameManager Instance = null;

    public PlayerController LocalPlayer = null;

    public RoundTimer Timer = null;
    public ScoreManager ScoreModule = null;

    public Dictionary<int, RobotStateMachine> PlayerList
        { get; protected set; }
    public Dictionary<int, RobotStateMachine> AlivePlayerList
        { get; protected set; }

    void Start() {
        GameObject TaggedTimer = GameObject.FindGameObjectWithTag("Timer");

        if (TaggedTimer == null) {
            Debug.LogError(this.GetType().Name + ": No Tagged Timer script found!");
        }
        else {
          this.Timer = TaggedTimer.GetComponent<RoundTimer>();
          if (this.Timer == null) Debug.Log("Merde");
          else Debug.Log("Yeah");
        }
    }

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

        while (!IsGameOver()) {
            if (Timer.remainingTime == 0) {
               endRoundWithTimer();
            }
        }
    }

    protected void endRoundWithTimer(){

        RobotStateMachine Winner = null;
        Winner = searchForMalHealthPlayers();
    }

    protected RobotStateMachine searchForMalHealthPlayers() {
       int MaxHP = 0;
       RobotStateMachine Winner = null;
       foreach(KeyValuePair<int,RobotStateMachine> alivePlayer in this.AlivePlayerList)
       {
          if (alivePlayer.Value.PlayerController.PlayerHealth.Health >= MaxHP) {
              Winner = alivePlayer.Value;
          }
       }
       return Winner;
    }

    protected virtual void Initialize() {
        this.AlivePlayerList = new Dictionary<int, RobotStateMachine>();
        this.PlayerList = new Dictionary<int, RobotStateMachine>();
        this.ScoreModule = this.gameObject.AddComponent<ScoreManager>();
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
        return this.AlivePlayerList.Count <= 1;
    }
}
