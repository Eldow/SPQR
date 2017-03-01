using UnityEngine;

public class RobotVictoryState : RobotState {
    private const float JumpTime = 1.208f;
    private const float Mass = 5000000f;
    private const float JumpForce = 20000000f;
    private float _oldMass = 1f;
    private float _currentTime = 0f;
    private Rigidbody _rigidbody = null;

    public override State HandleInput(StateMachine stateMachine) {
        return null;
    }

    public RobotVictoryState() {
        this.Initialize();
    }

    public override void Update(StateMachine stateMachine) {
        if (this._rigidbody == null) return;

        if ((this._currentTime += Time.fixedDeltaTime) <
            RobotVictoryState.JumpTime) {
            return;
        }

        this._currentTime = 0f;

        this._rigidbody.AddForce(new Vector3(
            0, 
            RobotVictoryState.JumpForce, 
            0), 
            ForceMode.Impulse);
        
        ((RobotStateMachine)stateMachine).PlayerController.PlayerPhysics
            .Move();
    }

    public override void Enter(StateMachine stateMachine) {
        this._currentTime = RobotVictoryState.JumpTime - 0.1f;

        if (!(stateMachine is RobotStateMachine)) return;

        RobotStateMachine robotStateMachine = (RobotStateMachine)stateMachine;
        this._rigidbody =
            robotStateMachine.gameObject.GetComponent<Rigidbody>();

        if (this._rigidbody == null) return;

        // changing his mass to make the jump more cartoonistic...
        this._oldMass = this._rigidbody.mass;
        this._rigidbody.mass = RobotVictoryState.Mass;

        // allows jump
        this._rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionY;
    }

    public override void Exit(StateMachine stateMachine) {
        if (this._rigidbody == null) return;

        // ... without forgetting of reverting it while Exiting the Win State.
        this._rigidbody.mass = this._oldMass;

        // disallows jump
        this._rigidbody.constraints &= RigidbodyConstraints.FreezePositionY;
    }
}
