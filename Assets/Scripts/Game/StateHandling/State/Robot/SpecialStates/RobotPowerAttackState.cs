using UnityEngine;

public class RobotPowerAttackState : RobotLoadedAttackState {
    protected float LoadingSpeed = .5f;

    protected override void Initialize() {
        this.MaxFrame = 30;
        this.IASA = 28;
        this.MinActiveState = 7;
        this.MaxActiveState = 13;
        this.Damage = 2;
        this.Hitstun = 10;
        this.HeatCost = 3;
        this.MaxLoadingFrame = 30;
        this.IsLoading = true;
        this.DamageMultiplier = 1.02f;
        this.RefreshRate = 5;
    }

    public override State HandleInput(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return null;

        RobotStateMachine robotStateMachine = (RobotStateMachine)stateMachine;

        if (!this.IsAnimationPlaying(robotStateMachine, "RobotPowerAttack")) {
            return null;
        }

        if (this.CheckIfPowerAttackHolding() && !this.IsAttackFullyLoaded()) {
            if (this.IsCurrentAnimationPlayedPast(robotStateMachine, .5f) &&
                Mathf.Abs(robotStateMachine.Animator.speed) > .01f) {
                this.FreezeAnimation(robotStateMachine);
            }

            return null;
        }

        this.IsLoading = false;
        this.SetLightings(false);
        this.ResumeNormalAnimation(robotStateMachine);

        if (this.IsInterruptible(robotStateMachine)) { // can be interrupted!
            RobotState newState = this.CheckInterruptibleActions();

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

    public RobotPowerAttackState() {
        this.Initialize();
    }

    public override void Update(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return;

        if (!this.IsLoading) this.CurrentFrame++;

        if (this.IsLoading) {
            this.UpdateCurrentLoadingFrame((RobotStateMachine)stateMachine);
        }

        ((RobotStateMachine)stateMachine).PlayerController.PlayerPhysics
            .Move();
    }

    protected virtual bool CheckIfPowerAttackHolding() {
        return InputManager.powerAttackButton();
    }

    public override void Enter(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return;

        RobotStateMachine robotStateMachine = (RobotStateMachine) stateMachine;

        base.Enter(stateMachine);

        this.SetAnimationSpeed(robotStateMachine, this.LoadingSpeed);
        this.SetLightings(true);
    }

    public override RobotState CheckInterruptibleActions() {
        if (InputManager.moveX() > .02f || InputManager.moveY() > .02f) {
            if (InputManager.runButton()) {
                return new RobotRunState();
            }

            return new RobotWalkState();
        }

        return null;
    }
}
