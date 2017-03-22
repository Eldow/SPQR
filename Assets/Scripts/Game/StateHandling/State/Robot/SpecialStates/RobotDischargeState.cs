using UnityEngine;

public class RobotDischargeState : RobotAttackState {
    protected float Radius = 10f;
    protected GameObject[] Enemies = null;

    protected override void Initialize() {
        this.AlreadyHitByAttack = false;
        this.MaxFrame = 30;
        this.IASA = 21;
        this.MinActiveState = 7;
        this.MaxActiveState = 13;
        this.Damage = 5;
        this.Hitstun = 10;
        this.HeatCost = 3;
    }

    public override State HandleInput(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return null;

        RobotStateMachine robotStateMachine = (RobotStateMachine)stateMachine;

        if (!this.IsAnimationPlaying(robotStateMachine, "RobotAttack1")) {
            return null;
        }

        if (InputManager.attackButton()) {
            return new RobotAttack2State();
        }
        

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

    public RobotDischargeState() {
        this.Initialize();
    }

    public override void Update(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return;

        this.CurrentFrame++;

        ((RobotStateMachine)stateMachine).PlayerController.PlayerPhysics
            .Move();
    }

    public override void Enter(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return;

        RobotStateMachine robotStateMachine = (RobotStateMachine) stateMachine;

        base.Enter(stateMachine);

        this.Enemies = GameObject.FindGameObjectsWithTag(
            PlayerController.Opponent);

        foreach (GameObject enemy in this.Enemies) {
            if (!this.CheckDistanceWithGameObject(robotStateMachine, enemy)) {
                continue;
            }

            this.MakeHimSuffer(robotStateMachine, enemy);
        }
    }

    public virtual bool CheckDistanceWithGameObject(
        RobotStateMachine robotStateMachine, GameObject gameObject) {
        return this.Radius >= 
            Vector3.Distance(
                robotStateMachine.transform.position, 
                gameObject.transform.position);
    }

    public virtual void MakeHimSuffer(
        RobotStateMachine robotStateMachine, GameObject enemy) {
        PlayerPhysics enemyPhysics = enemy.GetComponent<PlayerPhysics>();

        if (enemyPhysics == null) return;

        enemyPhysics.ApplyPoke(
            enemy.transform.position - robotStateMachine.transform.position);
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
