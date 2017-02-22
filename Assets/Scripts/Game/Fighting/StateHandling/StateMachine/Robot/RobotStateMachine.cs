using System;
using UnityEngine;
using UnityEngine.Networking;

public class RobotStateMachine : StateMachine {
    public Animator Animator = null;
    public PlayerController PlayerController = null;
    public NetworkAnimator NetworkAnimator = null;
    [HideInInspector] public FixedSizedQueue<string> StateHistory;
    public int MaxHistorySize = 12;

    // UGLY UGLY CODE BEARK
    private bool _isOver = false;

    // to be changed in a child class, if necessary
    public override string DefaultState {
        get { return "RobotIdleState"; }
    }

    void Update() {
        this.HandleInput();

        // TO BE FIXED, VERY UGLY
        if (PhotonNetwork.player.GetHealth() <= 0 && !this._isOver) {
            this.SetState(new RobotDefeatState());
            this._isOver = true;
        }

        if (PhotonNetwork.playerList[0].GetHealth() <= 0 && !this._isOver) {
            this.SetState(new RobotVictoryState());
            this._isOver = true;
        }
    }

    void Start() {
        this.Initialize();
    }

    protected override void Initialize(string startingState = null) {
        base.Initialize();

        this.Animator = this.GetComponent<Animator>();
        this.PlayerController = this.GetComponent<PlayerController>();
        this.NetworkAnimator = this.GetComponent<NetworkAnimator>();

        Type stateType = this.CheckStartingState(startingState);

        if (stateType == null) return;

        this.CurrentState = (RobotState) Activator.CreateInstance(stateType);
        this.StateHistory = new FixedSizedQueue<string>(this.MaxHistorySize);
    }

    protected override void SwitchState() {
        if (!(this.NextState is RobotState)) {
            return;
        }

        base.SwitchState();
        this.PlayerController.UpdateAnimations(
            this.CurrentState.GetType().Name);
    }
}
