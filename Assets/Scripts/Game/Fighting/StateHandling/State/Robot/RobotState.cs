public class RobotState : State {
    public override State HandleInput(StateMachine stateMachine) {
        return null;
    }

    public virtual bool IsAnimationPlaying(RobotStateMachine stateMachine,
        string animationName) {
        return stateMachine.Animator.GetCurrentAnimatorStateInfo(0)
            .IsName(animationName);
    }

    public virtual bool IsCurrentAnimationFinished(
        RobotStateMachine stateMachine) {
        return this.IsCurrentAnimationPlayedPast(stateMachine, 1);
    }

    public virtual bool IsCurrentAnimationPlayedPast(
        RobotStateMachine stateMachine, float normalizedTime = 1) {
        return stateMachine.Animator.GetCurrentAnimatorStateInfo(0)
                   .normalizedTime > normalizedTime &&
               !stateMachine.Animator.IsInTransition(0);
    }

    public virtual void FreezeAnimation(RobotStateMachine stateMachine) {
        this.SetAnimationSpeed(stateMachine, 0);
    }

    public virtual void ResumeAnimation(RobotStateMachine stateMachine) {
        this.SetAnimationSpeed(stateMachine, 1);
    }

    public virtual void SetAnimationSpeed(RobotStateMachine stateMachine,
        float speed = 1) {
        stateMachine.Animator.speed = speed;
    }

    public virtual void SaveToHistory(RobotStateMachine stateMachine) {
        stateMachine.StateHistory.Enqueue(this.GetType().Name);
    }

    public virtual bool IsLastState(RobotStateMachine stateMachine, 
        string lastStateGuessed) {
        return stateMachine.StateHistory.Peek() == 
            lastStateGuessed;
    }
}
