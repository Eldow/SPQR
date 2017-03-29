﻿using UnityEngine;

public class RobotDischargeState : RobotAttackState {
    protected float Radius = .10f;
    protected GameObject[] Enemies = null;
    protected SphereCollider AreaCollider = null;
    protected float RadiusGrowthRate = 0f;

    protected override void Initialize() {
        this.AlreadyHitByAttack = false;
        this.MaxFrame = 20;
        this.IASA = 18;
        this.MinActiveState = 0;
        this.MaxActiveState = this.MaxFrame;
        this.Damage = 5;
        this.Hitstun = 10;
        this.HeatCost = 10;
        this.ComputeRadiusGrowthRate();
    }

    public override State HandleInput(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return null;

        RobotStateMachine robotStateMachine = (RobotStateMachine)stateMachine;

        if (!this.IsAnimationPlaying(robotStateMachine, "RobotDischarge")) {
            return null;
        }

        if (!this.IsSpeedSet) this.SetSpeed(robotStateMachine);


        if (this.IsInterruptible(robotStateMachine)) { // can be interrupted!
            RobotState newState = this.CheckInterruptibleActions();

            if (newState != null) return newState;
        }

        if (this.IsStateFinished()) {
            return new RobotIdleState();
        }

        return null;
    }

    public RobotDischargeState() {
        this.Initialize();
    }

    public override void Update(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return;

        this.CurrentFrame++;

        if (this.AreaCollider == null) return;

        if (this.AreaCollider.radius >= this.Radius) {
            GameObject.Destroy(this.AreaCollider);
            this.AreaCollider = null;

            return;
        }

        this.AreaCollider.radius += this.RadiusGrowthRate;
    }

    public override void Enter(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return;

        RobotStateMachine robotStateMachine = (RobotStateMachine) stateMachine;

        base.Enter(stateMachine);

        this.SetLightings(true);
        this.AreaCollider 
            = robotStateMachine.gameObject.AddComponent<SphereCollider>();
        this.AreaCollider.isTrigger = true;
        this.AreaCollider.radius = this.Radius;
    }

    public override void Exit(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return;

        RobotStateMachine robotStateMachine = (RobotStateMachine)stateMachine;

        base.Exit(stateMachine);

        this.SetLightings(false);
        //robotStateMachine.PlayerController.PlayerPhysics.IsDischarged = true;

        if (this.AreaCollider == null) return;

        GameObject.Destroy(this.AreaCollider);
    }

    public virtual bool CheckDistanceWithGameObject(
        RobotStateMachine robotStateMachine, GameObject gameObject) {
        return this.Radius >= 
            Vector3.Distance(
                robotStateMachine.transform.position, 
                gameObject.transform.position);
    }

    public virtual void MakeHimSuffer(
        HandleHit player, Collider enemy) {
        PlayerPhysics enemyPhysics 
            = enemy.gameObject.GetComponent<PlayerPhysics>();

        if (enemyPhysics == null) return;

        player.SendPoke(enemy.gameObject, 
            (enemy.transform.position - player.transform.position).normalized);
    }

    public virtual void ComputeRadiusGrowthRate() {
        this.RadiusGrowthRate = this.Radius / this.MaxFrame;
    }

    public override RobotState CheckInterruptibleActions() {
        if (!(InputManager.moveX() > .02f) && !(InputManager.moveY() > .02f)) {
            return null;
        }

        if (InputManager.runButton()) {
            return new RobotRunState();
        }

        return new RobotWalkState();
    }

    public override void HandleAttackTrigger(HandleHit handleHit, Collider other) {
        if (this.AlreadyHitByAttack || !this.IsAttackActive())
            return;

        this.AlreadyHitByAttack = true;

        PlayerController opponent = 
            (PlayerController) other.gameObject
            .GetComponent<PlayerController>();

        if (opponent == null) return;

        this.SendAudioHit(opponent.PlayerAudio);
        handleHit.SendHit(other.gameObject, this.Damage, this.Hitstun);
        this.MakeHimSuffer(handleHit, other);
    }
}