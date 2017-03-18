using UnityEngine;

public class RobotState : State {
    public int HeatCost { get; protected set; }

    protected GameObject Lightnings = null;

    public override State HandleInput(StateMachine stateMachine) {
        return null;
    }

    protected override void Initialize() {
        base.Initialize();

        this.HeatCost = 0;
    }

    public RobotState() {
        this.GetLightning();
    }

    protected virtual void GetLightning() {
        GameObject player = GameObject.FindGameObjectWithTag(
            PlayerController.Player);
        Transform playerTransform = player.GetComponent<Transform>();

        if (playerTransform == null) return;

        foreach (Transform child in playerTransform) {
            if (child.CompareTag("Lightnings")) {
                this.Lightnings = child.gameObject;
            }
        }
    }

    protected virtual void SetLightings(bool isActive) {
        if (this.Lightnings != null) this.Lightnings.SetActive(isActive);
    }

    public override void Enter(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return;

        RobotStateMachine robotStateMachine = (RobotStateMachine) stateMachine;

        robotStateMachine.PlayerController.PlayerPower.Power -= this.HeatCost;

        if (robotStateMachine.PlayerController.PlayerPower.Power <= 0)
            robotStateMachine.SetState(new RobotOverheatState());

        PlayAudioEffect(robotStateMachine.PlayerController.PlayerAudio);
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

    public virtual void ResumeNormalAnimation(RobotStateMachine stateMachine) {
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

    public virtual void PlayAudioEffect(PlayerAudio audio) {}

    protected virtual void SetSpeed(RobotStateMachine robotStateMachine) {}
}
