public class RobotLoadedAttackState : RobotAttackState {
    protected int MaxLoadingFrame = 0;
    protected int CurrentLoadingFrame = 0;
    protected bool IsLoading = false;
    protected float DamageMultiplier = 1f;

    public RobotLoadedAttackState() {
        this.Initialize();
    }

    protected virtual bool IsAttachFullyLoaded() {
        return this.CurrentLoadingFrame >= this.MaxLoadingFrame;
    }

    protected virtual void UpdateCurrentLoadingFrame() {
        this.CurrentLoadingFrame++;

        this.Damage = (int) (this.Damage * this.DamageMultiplier);
    }
}
