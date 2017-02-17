using System;
using UnityEngine;

public class RobotStateMachine : StateMachine {
    public Animator Animator = null;
    public PlayerController PlayerController = null;
    [HideInInspector] public FixedSizedQueue<string> StateHistory;
    public int MaxHistorySize = 12;

    // to be changed in a child class, if necessary
    public override string DefaultState {
        get { return "RobotIdleState"; }
    }

    void Start() {
        this.Initialize();
    }

    void Update() {
        this.HandleInput(this.PlayerController.xboxInput);
    }

    protected override void Initialize(string startingState = null) {
        base.Initialize();

        this.Animator = this.GetComponent<Animator>();
        this.PlayerController = this.GetComponent<PlayerController>();

        Type stateType = this.CheckStartingState(startingState);

        if (stateType == null) return;

        this.CurrentState = (RobotState) Activator.CreateInstance(stateType);
        this.StateHistory = new FixedSizedQueue<string>(this.MaxHistorySize);
    }

    protected override void SwitchState(State state) {
        if (!(state is RobotState)) return;

        base.SwitchState(state);

        this.Animator.SetTrigger(state.GetType().Name);
    }
}
