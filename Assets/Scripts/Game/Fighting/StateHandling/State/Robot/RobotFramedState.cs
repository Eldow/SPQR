public class RobotFramedState : RobotState {
    public int IASA { get; protected set; }
    public int MaxFrame { get; protected set; }
    public int MinActiveState { get; protected set; }
    public int MaxActiveState { get; protected set; }
    public int CurrentFrame = 0;

    protected override void Initialize() {
        this.MaxFrame = 0;
        this.IASA = this.MaxFrame;
        this.MinActiveState = this.MaxFrame;
        this.MaxActiveState = this.MaxFrame;
    }

    public override void Update(StateMachine stateMachine) {
        this.CurrentFrame++;
    }

    public override string HandleInput(StateMachine stateMachine) {
        return null;
    }

    public virtual bool IsInterruptible(RobotStateMachine stateMachine) {
        return (this.CurrentFrame >= this.IASA);
    }

    public virtual bool IsStateFinished() {
        return this.CurrentFrame >= this.MaxFrame;
    }

    public virtual string CheckInterruptibleActions() {
        return null;
    }
}
