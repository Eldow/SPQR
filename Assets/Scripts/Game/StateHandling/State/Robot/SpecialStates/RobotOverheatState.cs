public class RobotOverheatState : RobotFramedState {
    protected int Duration = 200;

    protected override void Initialize() {
        this.MaxFrame = this.Duration;
        this.IASA = this.MaxFrame;
        this.MinActiveState = 0;
        this.MaxActiveState = this.MaxFrame;
        this.HeatCost = 0;
        this.InitialSpeed = 0;
        this.IsSpeedSet = false;
    }

    public RobotOverheatState() {
        this.Initialize();
    }

    public override State HandleInput(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return null;

        RobotStateMachine robotStateMachine = (RobotStateMachine) stateMachine;

        if (!this.IsAnimationPlaying(
            robotStateMachine, "RobotOverheat")) {
            return null;
        }

        if (!this.IsSpeedSet) this.SetSpeed(robotStateMachine);

        if (!this.IsStateFinished()) return null;

        if (this.IsLastState(robotStateMachine, "RobotWalkState")) {
            return new RobotWalkState();
        }

        return new RobotIdleState();
    }

    public override void Update(StateMachine stateMachine) {
        this.CurrentFrame++;
    }


    public override void Enter(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return;

        RobotStateMachine robotStateMachine = (RobotStateMachine) stateMachine;

        this.InitialSpeed = robotStateMachine.Animator.speed;

        PlayAudioEffect(robotStateMachine.PlayerController.PlayerAudio);

		this.SetLightings(stateMachine,true);
    }

    public override void Exit(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return;

        RobotStateMachine robotStateMachine = (RobotStateMachine) stateMachine;
		
		if( robotStateMachine.PlayerController.isAI ){
			robotStateMachine.PlayerController.PlayerPower.Power = 100;
		}

        robotStateMachine.Animator.speed = this.InitialSpeed;

		this.SetLightings(stateMachine, false);

        robotStateMachine.PlayerController.PlayerPower.Power = 
            PlayerPower.MaxPower;
    }

    protected override void SetSpeed(RobotStateMachine robotStateMachine) {
        robotStateMachine.Animator.speed = 2.5f;

        this.IsSpeedSet = true;
    }

    public override void PlayAudioEffect(PlayerAudio audio) {
        audio.Overload();
    }
}
