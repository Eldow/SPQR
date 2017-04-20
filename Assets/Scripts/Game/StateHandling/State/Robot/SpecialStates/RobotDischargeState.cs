using UnityEngine;

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
        this.Damage = 0;
        this.Hitstun = 60;
        this.HeatCost = 0;
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
			RobotState newState = this.CheckInterruptibleActions(stateMachine);

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

		this.SetLightings(stateMachine,true);
        this.AreaCollider 
            = robotStateMachine.gameObject.AddComponent<SphereCollider>();
        this.AreaCollider.isTrigger = true;
        this.AreaCollider.radius = 0f;
    }

    public override void Exit(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return;

        RobotStateMachine robotStateMachine = (RobotStateMachine)stateMachine;

        base.Exit(stateMachine);

		this.SetLightings(stateMachine,false);
        robotStateMachine.PlayerController.PlayerPhysics.IsDischarged = true;

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
        HandleDischarge player, Collider enemy) {
        PlayerPhysics enemyPhysics 
            = enemy.transform.root.gameObject.GetComponent<PlayerPhysics>();

        if (enemyPhysics == null) return;

        Vector3 direction = 
            (enemy.transform.position - player.transform.position).normalized;

        direction = Vector3.Project(direction, new Vector3(1, 0, 1));

        player.SendPoke(enemy.gameObject, direction);
    }

    public virtual void ComputeRadiusGrowthRate() {
        this.RadiusGrowthRate = this.Radius / this.MaxFrame;
    }

	public override RobotState CheckInterruptibleActions(StateMachine stateMachine) {

		RobotStateMachine robotStateMachine = (RobotStateMachine)stateMachine;

		InputManager inputManager = ((RobotStateMachine) stateMachine).PlayerController.inputManager;

		if (!(inputManager.moveX() > .02f) && !(inputManager.moveY() > .02f)) {
            return null;
        }

		if (inputManager.runButton()) {
            return new RobotRunState();
        }

        return new RobotWalkState();
    }

    public override void HandleAttackTrigger(HandleDischarge handleDischarge, Collider other) {
        if (this.AlreadyHitByAttack || !this.IsAttackActive())
            return;

        PlayerController opponent = 
            (PlayerController) other.transform.root.gameObject
            .GetComponent<PlayerController>();

        if (opponent == null) return;

        this.SendAudioHit(opponent.PlayerAudio);
        handleDischarge.SendHit(other.gameObject, this.Damage, this.Hitstun);
        this.MakeHimSuffer(handleDischarge, other);
        this.AlreadyHitByAttack = true;
    }

    public override void PlayAudioEffect(PlayerAudio audio)
    {
        audio.Discharge();
    }
}
