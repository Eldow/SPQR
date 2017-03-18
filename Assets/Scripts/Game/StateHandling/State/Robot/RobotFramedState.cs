using UnityEngine;

public class RobotFramedState : RobotState {
    public int IASA { get; protected set; }
    public int MaxFrame { get; protected set; }
    public int MinActiveState { get; protected set; }
    public int MaxActiveState { get; protected set; }
    public int CurrentFrame = 0;
    protected bool IsSpeedSet = false;
    protected float InitialSpeed = 0;

    protected override void Initialize() {
        this.MaxFrame = 0;
        this.IASA = this.MaxFrame;
        this.MinActiveState = this.MaxFrame;
        this.MaxActiveState = this.MaxFrame;
    }

    public override void Update(StateMachine stateMachine) {
        this.CurrentFrame++;
    }

    public override State HandleInput(StateMachine stateMachine) {
        return null;
    }

    public virtual bool IsInterruptible(RobotStateMachine stateMachine) {
        return (this.CurrentFrame >= this.IASA);
    }

    public virtual bool IsStateFinished() {
        return this.CurrentFrame >= this.MaxFrame;
    }

    public virtual RobotState CheckInterruptibleActions() {
        return null;
    }

    protected override void SetSpeed(RobotStateMachine robotStateMachine) {
        float desiredAnimationTime = this.MaxFrame / (1 / Time.fixedDeltaTime);

        robotStateMachine.Animator.speed =
            (robotStateMachine.Animator.GetCurrentAnimatorStateInfo(0).length *
            robotStateMachine.Animator.speed) / desiredAnimationTime;

        this.IsSpeedSet = true;
    }

    public override void Enter(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return;

        RobotStateMachine robotStateMachine = (RobotStateMachine)stateMachine;

        base.Enter(stateMachine);

        this.InitialSpeed = robotStateMachine.Animator.speed;
    }

    public override void Exit(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return;

        RobotStateMachine robotStateMachine = (RobotStateMachine)stateMachine;

        robotStateMachine.Animator.speed = this.InitialSpeed;
    }
}
