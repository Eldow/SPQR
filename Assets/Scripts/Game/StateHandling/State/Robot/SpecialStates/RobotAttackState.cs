using UnityEngine;

public class RobotAttackState : RobotFramedState {
    public int Damage { get; protected set; }
    public int Hitstun { get; protected set; }

    protected override void Initialize() {
        base.Initialize();

        this.Damage = 0;
        this.Hitstun = 0;
    }

    public virtual void HandleAttack(HandleHit handleHit, Collision other) {
        if (!this.IsAttackActive()) return;

        handleHit.SendHit(other, this.Damage, this.Hitstun);
    }

    public virtual bool IsAttackActive() {
        return this.CurrentFrame >=
            this.MinActiveState &&
            this.CurrentFrame <=
            this.MaxActiveState;
    }
}
