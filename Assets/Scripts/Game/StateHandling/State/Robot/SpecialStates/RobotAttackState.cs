public class RobotAttackState : RobotFramedState {
    public int Damage { get; protected set; }
    public int Hitstun { get; protected set; }

    protected override void Initialize() {
        base.Initialize();

        this.Damage = 0;
        this.Hitstun = 0;
    }
}
