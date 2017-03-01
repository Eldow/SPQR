using System;
using UnityEngine;
using UnityEngine.Networking;

public class RobotStateMachine : StateMachine {
    public Animator Animator = null;
    public PlayerController PlayerController = null;
    public NetworkAnimator NetworkAnimator = null;
    [HideInInspector] public FixedSizedQueue<string> StateHistory;
    public int MaxHistorySize = 12;

    // to be changed in a child class, if necessary
    public override string DefaultState {
        get { return "RobotIdleState"; }
    }

    void Update() {
        if (!this.photonView.isMine) return;

        this.HandleInput();
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
}
