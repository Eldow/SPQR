using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Running Running = null;
    public bool isLocalGame = true;
    public static GameManager Instance = null;

    public PlayerController LocalPlayer = null;

    public RoundTimer Timer = null;
    public Scoreboard Scores = null;

    public Dictionary<int, RobotStateMachine> PlayerList
    { get; protected set; }
    public Dictionary<int, RobotStateMachine> AlivePlayerList
    { get; protected set; }

    private bool exitStarted = false;
    public bool isCompletingRound = false;


    void Awake()
    {
        if (GameManager.Instance == null)
        {
            GameManager.Instance = this;
            this.Initialize();
        }
        else if (GameManager.Instance != this)
        {
            Destroy(gameObject);
        }

        if (this.Running != null) return;

        this.Running = this.gameObject.GetComponent<Running>();

        if (this.Running == null)
        {
            Debug.LogError(this.GetType().Name + ": No Running script found!");
        }
    }

    void Start()
    {
        GameObject TaggedTimer = GameObject.FindGameObjectWithTag("Timer");

        if (TaggedTimer == null)
        {
            Debug.LogError(this.GetType().Name + ": No Tagged Timer script found!");
        }
        else
        {
            this.Timer = TaggedTimer.GetComponent<RoundTimer>();
        }

        GameObject TaggedSb = GameObject.FindGameObjectWithTag("Score");

        if (TaggedSb == null)
        {
            Debug.LogError(this.GetType().Name + ": No Tagged Scoreboard script found!");
        }
        else
        {
            this.Scores = TaggedSb.GetComponent<Scoreboard>();
        }

        InvokeRepeating("WaitForPlayersToBeReady", 0f, 0.3f);
    }

    void WaitForPlayersToBeReady()
    {
        if (PlayerList.Count < NetworkGameManager.nbPlayersForThisGame)
            return;

        foreach (KeyValuePair<int, RobotStateMachine> player in this.PlayerList)
        {
            if (!player.Value.PlayerController.isPlayerReady)
            {
                return;
            }
        }

        Timer.callTimerRPC();
        CancelInvoke();
    }

    // Game ending, round ending management
    void FixedUpdate()
    {
        if (Timer != null && Timer.hasTimerStarted && Timer.remainingTime == 0f)
        {
            TimeoutEnding();
        }
    }

    protected virtual void Initialize()
    {
        this.AlivePlayerList = new Dictionary<int, RobotStateMachine>();
        this.PlayerList = new Dictionary<int, RobotStateMachine>();
    }

    public virtual void RemovePlayerFromGame(int playerID)
    {
        RobotStateMachine robotStateMachine = null;

        try
        {
            robotStateMachine = this.AlivePlayerList[playerID];
        }
        catch (KeyNotFoundException exception)
        {
            Debug.LogWarning(
                "RemovePlayerFromGame: key " + playerID + " was not found");
            Debug.LogWarning(exception.Message);
        }

        if (robotStateMachine != null)
        {
            this.AlivePlayerList.Remove(playerID);
        }

        robotStateMachine = this.PlayerList[playerID];

        if (robotStateMachine == null) return;

        this.PlayerList.Remove(playerID);
    }

    public virtual void AddPlayerToGame(PlayerController playerAvatar)
    {
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
        PlayerController playerController)
    {
        this.UpdateDeadList(playerController.ID);

        playerController.UpdateDeadToOthers();

        RobotStateMachine robotStateMachine =
             playerController.RobotStateMachine;

        if (robotStateMachine == null) return;

        robotStateMachine.SetState(new RobotDefeatState());
    }

    public virtual void UpdateDeadList(int playerID)
    {
        try
        {
            RobotStateMachine robotStateMachine = null;

            robotStateMachine = this.AlivePlayerList[playerID];

            if (robotStateMachine == null)
                return;

            this.AlivePlayerList.Remove(playerID);

            if (!this.IsLastTeamStanding())
                return;

            if (this.AlivePlayerList.Count <= 0)
                return;

            foreach (KeyValuePair<int, RobotStateMachine> winner in GameManager.Instance.AlivePlayerList)
            {
                if (winner.Value != null && winner.Value.PlayerController.photonView.isMine)
                    winner.Value.SetState(new RobotVictoryState());
            }
        }
        catch (KeyNotFoundException exception)
        {
            Debug.LogWarning(
                "UpdateDeadList: key " + playerID + " was not found");
            Debug.LogWarning(exception.Message);
        }
    }

    // If only only team left, declare the win
    public virtual bool IsLastTeamStanding()
    {
        int numberOfTeams = 1;
        string currentTeam, nextTeam;
        nextTeam = "";
        foreach (KeyValuePair<int, RobotStateMachine> player in GameManager.Instance.AlivePlayerList)
        {
            currentTeam = player.Value.PlayerController.Team;
            if (currentTeam != nextTeam)
            {
                if (nextTeam != "") { numberOfTeams++; }
                nextTeam = currentTeam;
            }
        }
        if (numberOfTeams > 1) return false;
        ManageEndRound(nextTeam);
        return true;
    }

    // Time off ! Get the best team an declare the win - TODO : Get team health points and not single player health points
    protected void TimeoutEnding()
    {
        RobotStateMachine Winner = null;
        Winner = SearchForMaxHealthPlayers();
        if (Winner.PlayerController.photonView.isMine)
            Winner.SetState(new RobotVictoryState());
        ManageEndRound(Winner.PlayerController.Team);
    }


    private RobotStateMachine SearchForMaxHealthPlayers()
    {
        int MaxHP = 0;
        RobotStateMachine Winner = null;
        foreach (KeyValuePair<int, RobotStateMachine> alivePlayer in this.AlivePlayerList)
        {
            if (alivePlayer.Value.PlayerController.PlayerHealth.Health >= MaxHP)
            {
                Winner = alivePlayer.Value;
                MaxHP = alivePlayer.Value.PlayerController.PlayerHealth.Health;
            }
        }
        return Winner;
    }

    private void ManageEndRound(string victoriousTeam)
    {
        if (isCompletingRound) return;
        isCompletingRound = true;
        if (victoriousTeam == null) return;
        if (Timer.Countdown != null)
        {
            Timer.Countdown.ManageKoSprite();
            Timer.photonView.RPC("ClientDisplayKo", PhotonTargets.AllViaServer);
        }
        if (PhotonNetwork.isMasterClient) Scores.AddVictory(victoriousTeam);
    }

    public void SetRoundFinished()
    {
        StartCoroutine(NextRound());
    }

    public void SetGameFinished()
    {
        Invoke("LeaveAfterEnding", 3.0f);
    }

    /*
        AUTO-LEAVE MANAGEMENT 
    */

    // Calls the right leaving routine
    private void LeaveAfterEnding()
    {
        if (PhotonNetwork.offlineMode)
        {
            StartCoroutine(LeaveTo("Launcher"));
            return;
        }
        if (PhotonNetwork.isMasterClient)
        {
            InvokeRepeating("LeaveAfterAll", 1f, 2f);
        }
        else
        {
            StartCoroutine(LeaveTo("Lobby"));
        }
    }

    // Prepare the scene for a new round
    IEnumerator NextRound()
    {
        yield return new WaitForSeconds(5f);
        PhotonNetwork.LoadLevel("Sandbox");
    }

    // Leave to launcher or lobby
    IEnumerator LeaveTo(string level)
    {
        yield return new WaitForSeconds(5f);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(level);
    }

    // Master leaves the room after others
    void LeaveAfterAll()
    {
        bool leaving = false;
        if (PhotonNetwork.room.PlayerCount == 1 && !leaving)
        {
            leaving = true;
            StartCoroutine(LeaveTo("Lobby"));
            CancelInvoke();
        }
    }
}
