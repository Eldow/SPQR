public class RobotAttack3State : RobotAttackState {
    protected int LastRefreshFrame = 0;
    public const int RefreshRate = 5;

    protected override void Initialize() {
        this.MaxFrame = 45;
        this.IASA = 38;
        this.MinActiveState = 8;
        this.MaxActiveState = 40;
        this.Damage = 1;
        this.Hitstun = 10;
        this.HeatCost = 20;
    }

    public override State HandleInput(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return null;

        RobotStateMachine robotStateMachine = (RobotStateMachine)stateMachine;

        if (!this.IsAnimationPlaying(robotStateMachine, "RobotAttack3")) {
            return null;
        }

        if (this.IsDischarge(robotStateMachine)) {
            return new RobotDischargeState();
        }

        if (this.IsInterruptible(robotStateMachine)) { // can be interrupted!
            RobotState newState = this.CheckInterruptibleActions(stateMachine);

            if (newState != null) return newState;
        }

        if (this.IsStateFinished()) {
            if (this.IsLastState(robotStateMachine, "RobotWalkState")) {
                return new RobotWalkState();
            }

            return new RobotIdleState();
        }

        return null;
    }

    public RobotAttack3State() {
        this.Initialize();
    }

    public override void Update(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return;

        this.CurrentFrame++;

        this.RefreshAttack();
        ((RobotStateMachine)stateMachine).PlayerController.PlayerPhysics
            .Move();
    }

    protected virtual void RefreshAttack() {
        if (this.CurrentFrame - this.LastRefreshFrame < RobotAttack3State.RefreshRate) return;

        this.LastRefreshFrame = this.CurrentFrame;
        AlreadyHitByAttack = false;
    }

    public override void Exit(StateMachine stateMachine) {
    }

    public override RobotState CheckInterruptibleActions(StateMachine stateMachine) {
		InputManager inputManager = ((RobotStateMachine) stateMachine).PlayerController.inputManager;
        if (inputManager.moveX() > .02f || inputManager.moveY() > .02f) {
            if (inputManager.runButton()) {
                return new RobotRunState();
            }

            return new RobotWalkState();
        }

        return null;
    }

    public override void PlayAudioEffect(PlayerAudio audio)
    {
        audio.Whirlwind();
    }
}
