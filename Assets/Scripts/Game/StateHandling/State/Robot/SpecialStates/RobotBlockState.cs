using UnityEngine;

public class RobotBlockState : RobotFramedState {

	protected GameObject Shield = null;
	public float shieldAngle = 70.0f;
	private bool isHolding = true;

    protected override void Initialize() {
        this.MaxFrame = 32;
        this.IASA = 8;
        this.MinActiveState = 6;
        this.MaxActiveState = 23;
        this.HeatCost = 3;
    }
		


    public override State HandleInput(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return null;

        RobotStateMachine robotStateMachine = (RobotStateMachine)stateMachine;

        if (!this.IsAnimationPlaying(robotStateMachine, "RobotBlock")) {
            return null;
        }

        if (this.CheckIfBlockHolding(stateMachine)) {
            if (this.IsCurrentAnimationPlayedPast(robotStateMachine, .5f) && 
                Mathf.Abs(robotStateMachine.Animator.speed) > .01f) {
                this.FreezeAnimation(robotStateMachine);
            }

            return null;
        }

        this.ResumeNormalAnimation(robotStateMachine);

        if (this.IsInterruptible(robotStateMachine)) { // can be interrupted!
            RobotState newState = this.CheckInterruptibleActions(stateMachine);

            if (newState != null) return newState;
        }

        if (this.IsStateFinished()) {
            return new RobotIdleState();
        }

        return null;
    }

    public RobotBlockState() {
        this.Initialize();
    }
		
    public override void Update(StateMachine stateMachine) {

		if (Shield == null)
			Shield = ((RobotStateMachine)stateMachine).PlayerController.Shield;
			
		if(this.CurrentFrame==this.MinActiveState && this.Shield != null)
			this.Shield.SetActive(true);

		this.CurrentFrame++;
		if (this.CheckIfBlockHolding (stateMachine)) {
			if (this.CurrentFrame >= MaxActiveState)
				this.CurrentFrame --;
		}

		if(this.CurrentFrame>=MaxActiveState)
		if (this.Shield != null) this.Shield.SetActive(false);

    }

    public override void Exit(StateMachine stateMachine) {
		if (this.Shield != null) this.Shield.SetActive(false);
    }


    protected virtual bool CheckIfBlockHolding(StateMachine stateMachine) {
		InputManager inputManager = ((RobotStateMachine) stateMachine).PlayerController.inputManager;
		if (isHolding) //If holding is released, you can't get back in
			isHolding =	inputManager.blockButton ();
		return isHolding;
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
        audio.Block();
    }
}
