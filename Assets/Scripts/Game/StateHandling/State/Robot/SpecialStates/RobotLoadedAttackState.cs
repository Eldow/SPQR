using UnityEngine;

public class RobotLoadedAttackState : RobotAttackState {
    protected int MaxLoadingFrame = 0;
    protected int CurrentLoadingFrame = 0;
    protected bool IsLoading = false;
    protected float DamageMultiplier = 1f;
    protected int RefreshRate = 0;

    public RobotLoadedAttackState() {
        this.Initialize();
    }

    protected virtual bool IsAttackFullyLoaded() {
        return this.CurrentLoadingFrame >= this.MaxLoadingFrame;
    }

    protected virtual void UpdateCurrentLoadingFrame(
        RobotStateMachine robotStateMachine) {
        this.CurrentLoadingFrame++;

        if (this.CurrentLoadingFrame % this.RefreshRate != 0) return;;

        this.Damage = Mathf.CeilToInt(this.Damage * this.DamageMultiplier);
        robotStateMachine.PlayerController.PlayerPower.Power -= this.HeatCost;
    }
}
