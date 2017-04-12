using System;
using UnityEngine;

public class RobotAttackState : RobotFramedState {
    public int Damage { get; protected set; }
    public int Hitstun { get; protected set; }
	protected bool AlreadyHitByAttack;
	

    protected override void Initialize() {	
        base.Initialize();
        this.Damage = 0;
        this.Hitstun = 0;
    }

    public virtual void HandleAttack(HandleHit handleHit, Collision other) {
		if (this.AlreadyHitByAttack || !this.IsAttackActive ())
			return;

		PlayerController opponent = (PlayerController)other.gameObject.GetComponent<PlayerController> ();


		if (opponent != null) {
			
			float angleBetweenRobots = Vector3.Angle (opponent.transform.forward, handleHit.transform.root.position - opponent.transform.position);

			//ignore hit
			if (opponent.RobotStateMachine.CurrentState is RobotBlockState && angleBetweenRobots < ((RobotBlockState)opponent.RobotStateMachine.CurrentState).shieldAngle)
				return;

			SendAudioHit (opponent.PlayerAudio);
			handleHit.SendHit (other.gameObject, this.Damage, this.Hitstun);
			
			//DEBUG
			ShowContact.ShowContactInstance.showContactPoint (other);
			//Debug.DrawRay (opponent.transform.position, opponent.transform.forward, Color.red, 3, false);
			//Debug.DrawRay (opponent.transform.position, handleHit.transform.root.position - opponent.transform.position, Color.blue, 3, false);
			//Debug.Log (angleBetweenRobots);

		}
		this.AlreadyHitByAttack = true;
	}

    public virtual void HandleAttackTrigger(HandleHit handleHit, Collider other) {
        if (this.AlreadyHitByAttack || !this.IsAttackActive())
            return;

        PlayerController opponent = (PlayerController)other.gameObject.GetComponent<PlayerController>();


        if (opponent != null) {
            Debug.Log("HANDLEATTACKTRIGGERFUCNTION " + opponent.ID + " " + DateTime.Now.ToShortTimeString());

            float angleBetweenRobots = Vector3.Angle(opponent.transform.forward, handleHit.transform.root.position - opponent.transform.position);
            
            if (opponent.RobotStateMachine.CurrentState is RobotBlockState && angleBetweenRobots > ((RobotBlockState)opponent.RobotStateMachine.CurrentState).shieldAngle)
                return;

            SendAudioHit(opponent.PlayerAudio);
            handleHit.SendHit(other.gameObject, this.Damage, this.Hitstun);

        }
        this.AlreadyHitByAttack = true;
    }

    public virtual bool IsAttackActive() {
        return this.CurrentFrame >=
            this.MinActiveState &&
            this.CurrentFrame <=
            this.MaxActiveState;
    }

    public override void PlayAudioEffect(PlayerAudio audio)
    {
        audio.Attack();
    }

    protected virtual void SendAudioHit(PlayerAudio audio)
    {
        if (audio == null) return;
        audio.Hit();
    }
}
